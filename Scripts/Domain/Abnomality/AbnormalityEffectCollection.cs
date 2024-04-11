using System.Collections.Generic;
using System.Linq;
using R3;

namespace Unity1week202403.Domain
{
    public class AbnormalityEffectCollection
    {
        public Observable<AbnormalityTypeCollection> OnUpdateAbnormalityType => _currentAbnormalityTypes;

        private readonly List<IAbnormalityEffectState> _collection = new();
        private readonly ReactiveProperty<AbnormalityTypeCollection> _currentAbnormalityTypes = new(AbnormalityTypeCollection.Empty);

        private float _currentTime;

        public void Add(params IAbnormalityEffectState[] effectState)
        {
            foreach (var abnormalityEffectState in effectState)
            {
                abnormalityEffectState.Start(_currentTime);
            }

            _collection.AddRange(effectState);

            var types = _collection
                .Select(x => x.Type)
                .Distinct()
                .ToArray();
            _currentAbnormalityTypes.Value = new AbnormalityTypeCollection(types);
        }

        public void Update(float currentTime)
        {
            _currentTime = currentTime;
            foreach (var effectState in _collection)
            {
                effectState.UpdateTime(currentTime);
            }

            // 効果が切れたものを削除
            var count = _collection.RemoveAll(x => !x.Effectable);
            if (count > 0)
            {
                _currentAbnormalityTypes.Value = new AbnormalityTypeCollection(
                    _collection.Select(x => x.Type).ToArray()
                );
            }
        }

        public bool IsMovable()
        {
            return !_collection.OfType<StanEffectState>().Any();
        }

        public bool IsAttackable()
        {
            return !_collection.OfType<StanEffectState>().Any();
        }
    }
}