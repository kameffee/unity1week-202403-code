using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Unity1week202403.Presentation
{
    public class GizmoDrawer : MonoBehaviour
    {
        [Inject]
        private readonly IReadOnlyList<IOnDrawGizmosHandler> _handlers;

        private void OnDrawGizmos()
        {
            foreach (var handler in _handlers)
            {
                handler.OnDrawGizmos();
            }
        }
    }
}