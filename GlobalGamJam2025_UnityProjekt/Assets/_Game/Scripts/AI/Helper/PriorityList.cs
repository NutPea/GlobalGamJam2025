using System;
using System.Collections.Generic;

namespace AI
{
    public class PriorityList<T, U>
    {
        /// <summary>
        /// Kapselt Item + Priority, nur intern verwendet.
        /// </summary>
        private class Entry
        {
            public T Item { get; }
            public U Priority { get; set; }

            public Entry(T item, U priority)
            {
                Item = item;
                Priority = priority;
            }
        }

        private readonly List<Entry> _entries = new List<Entry>();
        private readonly Dictionary<T, Entry> _lookup = new Dictionary<T, Entry>();
        private readonly IComparer<U> _comparer;

        // Flag, um anzuzeigen, ob seit letzter Sortierung etwas ge�ndert wurde
        private bool _isDirty = false;

        /// <summary>
        /// Erzeugt eine PriorityList mit optional eigenem Comparer.
        /// Wird keiner �bergeben, wird der Standard-Comparer benutzt.
        /// </summary>
        public PriorityList(IComparer<U> comparer = null)
        {
            _comparer = comparer ?? Comparer<U>.Default;
        }

        /// <summary>
        /// F�gt ein Item mit gegebener Priorit�t hinzu.
        /// </summary>
        public void Add(T item, U priority)
        {
            if (_lookup.ContainsKey(item))
                throw new ArgumentException($"Item '{item}' ist bereits enthalten.");

            var e = new Entry(item, priority);
            _entries.Add(e);
            _lookup[item] = e;
            _isDirty = true;
        }

        /// <summary>
        /// Gibt alle (Item, Priority)-Paare in der aktuell
        /// (eventuell noch nicht upgedateten) Reihenfolge zur�ck.
        /// Typischerweise willst du vorher 'UpdateSorting()' aufrufen,
        /// wenn du eine sortierte Ausgabe erwartest.
        /// 
        /// Du kannst w�hrend des Iterierens Priorit�ten �ndern.
        /// Dann ist _isDirty wieder true, 
        /// was bedeutet, dass du nach dem Iterieren ggf. 'UpdateSorting()' aufrufen solltest.
        /// </summary>
        public IEnumerable<(T item, U priority)> Items
        {
            get
            {
                // Kein automatisches Sortieren hier � sonst w�rde jedes foreach
                // unn�tig sortieren, was du ja explizit vermeiden willst.
                // Falls du IMMER sortiert iterieren m�chtest,
                // ruf vorher manuell UpdateSorting() auf.
                foreach (var e in _entries)
                {
                    yield return (e.Item, e.Priority);
                }
            }
        }

        /// <summary>
        /// L�scht alle Items.
        /// </summary>
        public void Clear()
        {
            _entries.Clear();
            _lookup.Clear();
            _isDirty = false;
        }

        /// <summary>
        /// Setzt die Priorit�t eines vorhandenen Items.
        /// Hier wird keine sofortige Neusortierung ausgel�st.
        /// </summary>
        public void SetPriority(T item, U newPriority)
        {
            if (_lookup.TryGetValue(item, out var entry))
            {
                entry.Priority = newPriority;
                _isDirty = true;
            }
            else
            {
                throw new KeyNotFoundException($"Item '{item}' ist nicht in der PriorityList vorhanden.");
            }
        }

        /// <summary>
        /// F�hrt die Sortierung nach allen bisher ge�nderten Priorit�ten durch.
        /// Erst danach sind 'Items' wirklich in aktueller Reihenfolge.
        /// </summary>
        public void UpdateSorting()
        {
            if (!_isDirty)
                return;

            _entries.Sort((a, b) => _comparer.Compare(a.Priority, b.Priority));
            _isDirty = false;
        }

        /// <summary>
        /// Gibt das erste Item in der aktuellen Sortierung zur�ck (Item, Priority).
        /// Falls _isDirty true ist, wird erst UpdateSorting() aufgerufen.
        /// Wirft eine Exception bei leerer Liste.
        /// </summary>
        public (T item, U priority) First
        {
            get
            {
                if (_entries.Count == 0)
                    throw new InvalidOperationException("PriorityList ist leer.");

                if (_isDirty)
                    UpdateSorting();

                var e = _entries[0];
                return (e.Item, e.Priority);
            }
        }

        /// <summary>
        /// Anzahl der enthaltenen Items.
        /// </summary>
        public int Count => _entries.Count;
    }
}