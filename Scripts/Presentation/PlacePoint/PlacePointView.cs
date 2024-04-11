using UnityEngine;

namespace Unity1week202403.Presentation
{
    public class PlacePointView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _placeablePoint;

        [SerializeField]
        private GameObject _unplaceablePoint;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Show(bool isPlaceable)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            SetPlaceable(isPlaceable);
        }

        public void UpdatePosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }
        
        public void SetRadius(float radius)
        {
            _placeablePoint.transform.localScale = Vector3.one * radius * 2;
            _unplaceablePoint.transform.localScale = Vector3.one * radius * 2;
        }

        private void SetPlaceable(bool isPlaceable)
        {
            if (isPlaceable)
            {
                _placeablePoint.SetActive(true);
                _unplaceablePoint.SetActive(false);
            }
            else
            {
                _placeablePoint.SetActive(false);
                _unplaceablePoint.SetActive(true);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}