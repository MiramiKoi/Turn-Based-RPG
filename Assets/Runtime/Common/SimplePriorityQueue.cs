using System.Collections.Generic;

namespace Runtime.Common
{
    public class SimplePriorityQueue<T>
    {
        private readonly List<(T item, int priority)> _items = new();

        public int Count => _items.Count;

        public void Enqueue(T item, int priority)
        {
            _items.Add((item, priority));
        }

        public T Dequeue()
        {
            var bestIndex = 0;

            for (var i = 1; i < _items.Count; i++)
            {
                if (_items[i].priority < _items[bestIndex].priority)
                    bestIndex = i;
            }

            var bestItem = _items[bestIndex].item;
            _items.RemoveAt(bestIndex);
            return bestItem;
        }
    }
}