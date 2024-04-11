using System;
using Cinemachine;
using UnityEngine;

namespace Unity1week202403.Presentation
{
    public class ResultVirtualCamera : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _virtualCamera;

        public void SetActive(bool active)
        {
            _virtualCamera.gameObject.SetActive(active);
        }

        public void SetFollowTarget(Transform target)
        {
            _virtualCamera.Follow = target;
        }

        private void OnValidate()
        {
            if (_virtualCamera == null)
            {
                _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            }
        }
    }
}