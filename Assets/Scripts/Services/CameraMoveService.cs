using Services.InputRaycastService;
using UnityEngine;
using Views;
using Zenject;

namespace Services
{
    public class CameraMoveService : ITickable
    {
        private const float ZOOM_SPEED = 3f;
        private const float MIN_CAMERA_HEIGHT = 3f;
        private const float MAX_CAMERA_HEIGHT = 35f;
        
        private readonly Camera m_camera;
        private readonly IInputRaycastService m_inputRaycastService;

        private readonly Plane m_groundPlane = new(Vector3.up, Vector3.zero);

        private bool m_isPanning;
        private Vector3 m_previousGroundPoint;

        public CameraMoveService(
            Camera _camera,
            IInputRaycastService _inputRaycastService)
        {
            m_camera = _camera;
            m_inputRaycastService = _inputRaycastService;
        }

        public void Tick()
        {
            HandlePan();
            HandleZoom();
        }

        private void HandlePan()
        {
            if (Input.GetMouseButtonDown(0))
                TryStartPan();

            if (m_isPanning && Input.GetMouseButton(0))
                UpdatePan();

            if (m_isPanning && Input.GetMouseButtonUp(0))
                m_isPanning = false;
        }

        private void TryStartPan()
        {
            if (IsPointerOverInteractiveObject())
                return;

            if (!TryGetMousePointOnGround(out var groundPoint))
                return;

            m_isPanning = true;
            m_previousGroundPoint = groundPoint;
        }

        private void UpdatePan()
        {
            if (!TryGetMousePointOnGround(out var currentGroundPoint))
                return;

            var delta = m_previousGroundPoint - currentGroundPoint;
            m_camera.transform.position += delta;

            if (TryGetMousePointOnGround(out var recalculatedGroundPoint))
                m_previousGroundPoint = recalculatedGroundPoint;
        }

        private void HandleZoom()
        {
            var scroll = Input.mouseScrollDelta.y;

            if (Mathf.Approximately(scroll, 0f))
                return;

            if (!TryGetMousePointOnGround(out var targetPoint))
                return;

            var direction = (targetPoint - m_camera.transform.position).normalized;
            var nextPosition = m_camera.transform.position + direction * (scroll * ZOOM_SPEED);

            if (nextPosition.y < MIN_CAMERA_HEIGHT || nextPosition.y > MAX_CAMERA_HEIGHT)
                return;

            m_camera.transform.position = nextPosition;
        }

        private bool IsPointerOverInteractiveObject()
        {
            if (!m_inputRaycastService.RaycastFromMouse(out var hit))
                return false;

            if (hit.collider.GetComponentInParent<PawnView>() != null)
                return true;

            if (hit.collider.GetComponentInParent<ConnectorView>() != null)
                return true;

            return false;
        }

        private bool TryGetMousePointOnGround(out Vector3 _point)
        {
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);

            if (m_groundPlane.Raycast(ray, out var distance))
            {
                _point = ray.GetPoint(distance);
                _point.y = 0f;
                return true;
            }

            _point = default;
            return false;
        }
    }
}