using TMPro;
using UnityEngine;

namespace Unity1week202403.Presentation
{
    public class CurrentCostView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _currentCost;

        public void UpdateCost(int cost)
        {
            _currentCost.text = cost.ToString();
        }
    }
}