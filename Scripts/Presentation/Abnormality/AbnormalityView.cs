using System;
using System.Collections.Generic;
using System.Linq;
using Unity1week202403.Data;
using Unity1week202403.Domain;
using UnityEngine;

namespace Unity1week202403.Presentation
{
    public class AbnormalityView : MonoBehaviour
    {
        [SerializeField]
        private Transform _holder;

        [SerializeField]
        private List<Pair> _pairs;

        private readonly Dictionary<AbnormalityType, GameObject> _abnormalityCache = new();

        public void UpdateAbnormality(AbnormalityTypeCollection abnormalityTypeCollection)
        {
            foreach (var abnormalityType in abnormalityTypeCollection.Types)
            {
                Show(abnormalityType);
            }

            foreach (var abnormalityType in _abnormalityCache.Keys.ToArray())
            {
                if (!abnormalityTypeCollection.Contains(abnormalityType))
                {
                    Hide(abnormalityType);
                }
            }
        }

        private void Show(AbnormalityType type)
        {
            // 既に表示されている場合は何もしない
            if (_abnormalityCache.ContainsKey(type))
                return;

            var prefab = GetPrefab(type);
            _abnormalityCache.Add(type, Instantiate(prefab, transform));
        }

        private void Hide(AbnormalityType type)
        {
            if (!_abnormalityCache.ContainsKey(type))
                return;

            Destroy(_abnormalityCache[type]);
            _abnormalityCache.Remove(type);
        }

        private GameObject GetPrefab(AbnormalityType type)
        {
            return _pairs.Find(x => x.Type == type).Prefab;
        }

        [Serializable]
        public class Pair
        {
            public AbnormalityType Type;
            public GameObject Prefab;
        }
    }
}