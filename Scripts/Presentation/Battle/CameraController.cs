using System;
using Cinemachine;
using UnityEngine;

namespace Unity1week202403.Presentation
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _virtualCamera;

        [SerializeField]
        private float _speed = 5f;

        [SerializeField]
        private Transform _followTarget;

        [SerializeField]
        private float _zoomSensitivity = 1f;

        private bool _isPlayable;
        private float _defaultCameraDistance;
        private CinemachineFramingTransposer _framingTransposer;

        private void Awake()
        {
            _framingTransposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            _defaultCameraDistance = _framingTransposer.m_CameraDistance;
        }

        public void SetPlayable(bool isPlayable)
        {
            _isPlayable = isPlayable;
        }

        public void ResetPosition()
        {
            _followTarget.position = Vector3.zero;
            _framingTransposer.m_CameraDistance = _defaultCameraDistance;
        }

        private void Update()
        {
            if (!_isPlayable) return;

            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            Move(horizontal, vertical);

            // マウスホイールでズーム
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            Zoom(scroll);
        }

        private void Zoom(float scroll)
        {
            var componentBase = _virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
            if (componentBase is CinemachineFramingTransposer transposer)
            {
                transposer.m_CameraDistance =
                    Mathf.Clamp(transposer.m_CameraDistance + scroll * _zoomSensitivity, 0, 40); // your value
            }
        }

        private void Move(float horizontal, float vertical)
        {
            var direction = new Vector3(horizontal, 0, vertical);
            _followTarget.position += direction.normalized * _speed * Time.unscaledDeltaTime;
        }
    }
}