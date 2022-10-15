using NoiseAnalyzer.Core.AcousticModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundDoc.Core
{
    public class AnalysisMatrix : IEnumerable<AcuItem>
    {
        private readonly List<AcuItem> _items = new();

        public int ItemsCount => _items.Count;

        public AcuItem this[int index] => _items[index];

        public void AddItem(AcuItem item)
        {
            _items.Add(item);
        }

        public void AddItem(AcuItem item, int index)
        {
            _items.Insert(index, item);
        }

        public void AddAll(params AcuItem[] items) 
        {
            foreach(AcuItem item in items) 
            {
                AddItem(item);
            }
        }

        public void RemoveLastItem()
        {
            ThrowIfItemsEmpty();

            _items.RemoveAt(_items.Count - 1);

            RecalculateAll();
        }

        public void RemoveItem(int index)
        {
            ThrowIfItemsEmpty();

            _items.RemoveAt(index);

            RecalculateAll();
        }

        public void RemoveAllItems()
        {
            _items.Clear();

            RecalculateAll();
        }

        private void ThrowIfItemsEmpty()
        {
            if (!_items.Any())
            {
                throw new InvalidOperationException("There are no items");
            }
        }

        public void RecalculateAll()
        {
            if (!_items.Any())
            {
                return;
            }

            for (int i = 1; i < _items.Count; i++)
            {
                _items[i].SetInputLw(_items[i - 1].OutputLw);
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var acuItem in _items)
            {
                builder.AppendLine(acuItem.ToString());
            }

            return builder.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<AcuItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}