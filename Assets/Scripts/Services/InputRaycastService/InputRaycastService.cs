using UnityEngine;

namespace Services.InputRaycastService
{
    public class InputRaycastService : IInputRaycastService
    {
        private readonly Camera m_camera;
        private readonly Plane m_groundPlane = new(Vector3.up, Vector3.zero);

        public InputRaycastService(Camera _camera)
        {
            m_camera = _camera;
        }

        public bool RaycastFromMouse(out RaycastHit _hit)
        {
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out _hit);
        }

        public bool TryGetMousePointOnGround(out Vector3 _point)
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