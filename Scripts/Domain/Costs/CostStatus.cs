using R3;
using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Domain
{
    public class CostStatus
    {
        public Cost Current => _reactiveCost.Value;
        public ReadOnlyReactiveProperty<Cost> ReactiveCurrent => _reactiveCost;

        private readonly ReactiveProperty<Cost> _reactiveCost = new();

        public CostStatus(Cost initial)
        {
            _reactiveCost.Value = initial;
        }

        public void Consume(Cost cost)
        {
            _reactiveCost.Value = new Cost(Mathf.Max(0, Current.Value - cost.Value));
        }

        public void Add(Cost cost)
        {
            _reactiveCost.Value = new Cost(Current.Value + cost.Value);
        }
    }
}