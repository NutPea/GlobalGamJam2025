using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public interface IMovementProvider
    {
        /// <summary>
        /// Gibt Hashset an Tatsächlich begehbaren Positionen zurück -> an Grid anfragen ob frei ist bevor ich was zurück gebe
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        HashSet<Vector2Int> GetMovementPossibilites(Vector2Int position);
    }
}