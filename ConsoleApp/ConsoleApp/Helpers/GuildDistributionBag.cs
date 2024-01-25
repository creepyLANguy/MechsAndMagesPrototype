using System.Collections.Generic;
using MaM.Enums;

namespace MaM.Helpers
{
    //TODO - implement as a generic distribution bag
    public class GuildDistributionBag
    {
        private readonly List<Guild> _items;
        private readonly Dictionary<Guild, int> _distributionTemplate;
        private readonly bool _shuffleBagOnFill;
        private readonly bool _shuffleBagOnTake;

        public GuildDistributionBag(Dictionary<Guild, int> distributionTemplate, bool shuffleBagOnFill = true, bool shuffleBagOnTake = false)
        {
            _items = new List<Guild>();

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

        public Guild Take()
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