using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI
{
    public static class AStarHelper
    {
        /// <summary>
        /// Berechnet mit A* den minimalen Pfad vom Start bis zum zuerst gefundenen Ziel,
        /// sobald ein Ziel erreicht werden kann. (Ziele stehen in 'targets'.)
        /// 
        /// Rückgabe:
        ///  - Item1: Liste der Koordinaten des Pfads (ohne Startpunkt, aber inkl. Zielpunkt)
        ///  - Item2: Gesamtkosten
        /// </summary>
        public static (List<Vector2Int>, float) CalculatePath(
            List<Vector2Int> targets,
            Vector2Int start,
            Func<Vector2Int, bool> moveableCheck)
        {
            // Edge Case: Keine Ziele angegeben
            if (targets == null || targets.Count == 0)
            {
                return (new List<Vector2Int>(), float.MaxValue);
            }

            // Falls Start selbst schon in targets liegt – je nach Wunsch direkt (leerer Pfad, cost=0)
            // oder normal weiter A*. Hier ignorieren wir es mal und lassen A* laufen.

            // A* Datenstrukturen:
            var openSet = new HashSet<Vector2Int> { start };              // "offene" Knoten
            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();      // Pfad-Rückverfolgung
            var gScore = new Dictionary<Vector2Int, float>();           // Kosten vom Start zu diesem Feld
            var fScore = new Dictionary<Vector2Int, float>();           // gScore + Heuristik

            gScore[start] = 0f;
            fScore[start] = Heuristic(start, targets);

            while (openSet.Count > 0)
            {
                // Nimm das Feld mit dem kleinsten fScore
                var current = openSet.OrderBy(n => fScore[n]).First();

                // Ist current eines der Ziele? Dann haben wir den minimalen Pfad gefunden.
                if (targets.Contains(current))
                {
                    float cost = gScore[current];
                    // Pfad von start bis current rekonstruieren
                    var path = ReconstructPath(cameFrom, current);
                    // Startpunkt entfernen (falls du ihn nicht im Pfad haben willst):
                    if (path.Count > 0 && path[0] == start)
                    {
                        path.RemoveAt(0);
                    }
                    return (path, cost);
                }

                openSet.Remove(current);

                // 4 mögliche Nachbarn (up, down, left, right)
                foreach (var neighbor in GetNeighbors(current))
                {
                    // Prüfen, ob das Nachbarfeld betretbar ist
                    if (!moveableCheck(neighbor))
                        continue;

                    float tentativeG = gScore[current] + 1f;

                    if (!gScore.TryGetValue(neighbor, out float oldG) || tentativeG < oldG)
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeG;
                        fScore[neighbor] = tentativeG + Heuristic(neighbor, targets);
                        openSet.Add(neighbor);
                    }
                }
            }

            // Wenn wir hierher kommen, ist kein Ziel erreichbar:
            return (new List<Vector2Int>(), float.MaxValue);
        }

        /// <summary>
        /// Berechnet die Manhattan-Heuristik eines Feldes zum *nächsten* der Ziele,
        /// indem wir den minimalen Manhattan-Abstand zu allen Targets verwenden.
        /// </summary>
        private static float Heuristic(Vector2Int pos, List<Vector2Int> targets)
        {
            float minDist = float.MaxValue;
            foreach (var t in targets)
            {
                float d = Math.Abs(pos.x - t.x) + Math.Abs(pos.y - t.y);
                if (d < minDist)
                    minDist = d;
            }
            return minDist;
        }

        /// <summary>
        /// Rekonstruiert den Pfad vom 'start' (implizit) bis 'current' 
        /// mithilfe des cameFrom-Dictionaries.
        /// Start ist am Pfadanfang, current am Ende.
        /// </summary>
        private static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            var path = new List<Vector2Int> { current };
            while (cameFrom.TryGetValue(current, out Vector2Int prev))
            {
                current = prev;
                path.Add(current);
            }
            path.Reverse(); // jetzt ist start am Index 0, current (Ziel) am letzten Index
            return path;
        }

        /// <summary>
        /// Hilfsfunktion: gibt die 4 orthogonalen Nachbarn zurück.
        /// </summary>
        private static IEnumerable<Vector2Int> GetNeighbors(Vector2Int current)
        {
            yield return new Vector2Int(current.x + 1, current.y);
            yield return new Vector2Int(current.x - 1, current.y);
            yield return new Vector2Int(current.x, current.y + 1);
            yield return new Vector2Int(current.x, current.y - 1);
        }
    }
}