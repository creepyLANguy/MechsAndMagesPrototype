using System.Collections.Generic;

namespace MaM.Helpers
{
  public class DistributionBag<T>
  {
        private readonly List<T> _items;
        private readonly Dictionary<T, int> _distributionTemplate;
        private readonly bool _shuffleBagOnFill;
        private readonly bool _shuffleBagOnTake;

        public DistributionBag(Dictionary<T, int> distributionTemplate, bool shuffleBagOnFill = true, bool shuffleBagOnTake = false)
        {
            _items = new List<T>();

            _distributionTemplate = distributionTemplate;

            _shuffleBagOnFill = shuffleBagOnFill;
            _shuffleBagOnTake = shuffleBagOnTake;

            Fill();
        }

        private void Fill()
        {
            foreach (var pair in _distributionTemplate)
            {
                for (var i = 0; i < pair.Value; i++)
                {
                    _items.Add(pair.Key);
                }
            }

            if (_shuffleBagOnFill)
            {
                _items.Shuffle();
            }
        }

        public T Take()
        {
            if (_items.Count == 0)
            {
                Fill();
            }

            if (_shuffleBagOnTake)
            {
                _items.Shuffle();
            }

            var chosen = _items![0];
            _items.RemoveAt(0);
            return chosen;
        }
    }
}