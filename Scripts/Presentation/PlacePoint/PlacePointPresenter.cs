using Unity1week202403.Extensions;
using UnityEngine;

namespace Unity1week202403.Presentation
{
    public class PlacePointPresenter : Presenter
    {
        private readonly PlacePointView _view;

        public PlacePointPresenter(PlacePointView view)
        {
            _view = view;
        }

        public void Show(Vector3 worldPosition, float radius, bool isPlaceable)
        {
            _view.UpdatePosition(worldPosition);
            _view.SetRadius(radius);
            _view.Show(isPlaceable);
        }

        public void Hide()
        {
            _view.Hide();
        }
    }
}