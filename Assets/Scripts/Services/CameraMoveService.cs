using Services.InputRaycastService;
using Services.InteractionService;
using Settings;
using UnityEngine;
using Views;
using Zenject;

namespace Services
{
    public class CameraMoveService : ITickable
    {
        private readonly Camera m_camera;
        private readonly CameraMoveSettings m_settings;
        private readonly IInputRaycastService m_inputRaycastService;
        private readonly IInteractionStateService m_interactionStateService;

        private readonly Plane m_groundPlane = new(Vector3.up, Vector3.zero);
        
        private Vector3 m_previousGroundPoint;

        public CameraMoveService(
            Camera _camera,
            CameraMoveSettings _settings,
            IInputRaycastService _inputRaycastService,
            IInteractionStateService _interactionStateService)
        {
            m_camera = _camera;
            m_settings = _settings;
            m_inputRaycastService = _inputRaycastService;
            m_interactionStateService = _interactionStateService;
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

            if (m_interactionStateService.isCameraPanning && Input.GetMouseButton(0))
                UpdatePan();

            if (m_interactionStateService.isCameraPanning && Input.GetMouseButtonUp(0))
                m_interactionStateService.End(InteractionMode.CAMERA_PAN);
        }

        private void TryStartPan()
        {
            if (!m_interactionStateService.isFree)
                return;
            
            if (IsPointerOverInteractiveObject())
                return;

            if (!TryGetMousePointOnGround(out var groundPoint))
                return;

            if (!m_interactionStateService.TryBegin(InteractionMode.CAMERA_PAN))
                return;
            
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
            var nextPosition = m_camera.transform.position + direction * (scroll * m_settings.zoomSpeed);

            if (nextPosition.y < m_settings.minCameraHeight || nextPosition.y > m_settings.maxCameraHeight)
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