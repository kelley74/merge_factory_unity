using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core
{
    public class DeckHolder
    {
        private Vector3[] _spotPositions;
        private int _currentIndex;
        private List<IMatchItem> _matchItems;
        private readonly int _mergeAmount = 3;

        public bool IsFull { get; private set; }

        public void Init(Vector3[] positions)
        {
            _spotPositions = positions;
            _currentIndex = 0;
            IsFull = false;
            _matchItems = new List<IMatchItem>();
        }

        public Vector3 GetNextPosition()
        {
            return _spotPositions[_currentIndex];
        }

        public void RegisterNewItem(IMatchItem item)
        {
            var items = _matchItems.Select(t => t.Id).ToList();
            if (items.Contains(item.Id))
            {
                var pos = items.LastIndexOf(item.Id);
                var amount = _matchItems.Count(t => t.Id == item.Id);

                if (pos == _matchItems.Count - 1)
                {
                    _matchItems.Add(item);
                }
                else
                {
                    _matchItems.Insert(pos + 1, item);
                }

                if (amount == _mergeAmount - 1)
                {
                    var startPos = pos - 1;
                    var itemsToMerge = new List<IMatchItem>();
                    for (int i = startPos; i < startPos + _mergeAmount; i++)
                    {
                        itemsToMerge.Add(_matchItems[i]);
                    }

                    foreach (var itemToMerge in itemsToMerge)
                    {
                        itemToMerge.MatchAt(_spotPositions[pos]);
                        _matchItems.Remove(itemToMerge);
                    }

                    _currentIndex -= _mergeAmount;
                }

                Reorder();
            }
            else
            {
                _matchItems.Add(item);
            }

            if (_matchItems.Count == _spotPositions.Length)
            {
                IsFull = true;
            }

            _currentIndex++;
        }

        private void Reorder()
        {
            for (int i = 0; i < _matchItems.Count; i++)
            {
                _matchItems[i].AlignAt(_spotPositions[i]);
            }
        }
    }
}