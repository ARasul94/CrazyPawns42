using Services.InputRaycastService;
using UnityEngine;
using Views;

namespace Services.ConnectionPreviewService
{
    public class ConnectionPreviewService : IConnectionPreviewService
    {
        private const float LINE_WIDTH = 0.07f;

        private readonly Camera m_camera;
        private readonly IInputRaycastService m_inputRaycastService;
        private readonly Transform m_root;
        private readonly Material m_lineMaterial;

        private ConnectorView m_from;
        private LineRenderer m_lineRenderer;
        private GameObject m_lineObject;

        public bool isActive => m_lineRenderer != null && m_from != null;

        public ConnectionPreviewService(
            Camera _camera,
            IInputRaycastService _inputRaycastService,
            Transform _root)
        {
            m_camera = _camera;
            m_inputRaycastService = _inputRaycastService;
            m_root = _root;
            m_lineMaterial = CreateLineMaterial();
        }

        public void Show(ConnectorView _from)
        {
            Hide();

            if (_from == null)
                return;

            m_from = _from;

            m_lineObject = new GameObject("Connection Preview Line");
            m_lineObject.transform.SetParent(m_root, false);

            m_lineRenderer = m_lineObject.AddComponent<LineRenderer>();
            m_lineRenderer.useWorldSpace = true;
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.startWidth = LINE_WIDTH;
            m_lineRenderer.endWidth = LINE_WIDTH;
            m_lineRenderer.numCapVertices = 4;
            m_lineRenderer.material = m_lineMaterial;

            Update();
        }

        public void Update()
        {
            if (!isActive)
                return;

            var startPosition = m_from.cachedTransform.position;
            var endPosition = GetPreviewEndPosition(startPosition);

            m_lineRenderer.SetPosition(0, startPosition);
            m_lineRenderer.SetPosition(1, endPosition);
        }

        public void Hide()
        {
            m_from = null;
            m_lineRenderer = null;

            if (m_lineObject != null)
                Object.Destroy(m_lineObject);

            m_lineObject = null;
        }

        private Vector3 GetPreviewEndPosition(Vector3 _startPosition)
        {
            var connectorUnderMouse = TryGetConnectorUnderMouse();

            if (connectorUnderMouse != null)
                return connectorUnderMouse.cachedTransform.position;

            return GetMousePointOnPlaneAtHeight(_startPosition.y);
        }

        private ConnectorView TryGetConnectorUnderMouse()
        {
            if (!m_inputRaycastService.RaycastFromMouse(out var hit))
                return null;

            return hit.collider.GetComponent<ConnectorView>();
        }

        private Vector3 GetMousePointOnPlaneAtHeight(float _y)
        {
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(Vector3.up, new Vector3(0f, _y, 0f));

            if (plane.Raycast(ray, out var distance))
                return ray.GetPoint(distance);

            return m_from.cachedTransform.position;
        }

        private static Material CreateLineMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            if (shader == null)
                shader = Shader.Find("Sprites/Default");

            var material = new Material(shader);
            material.color = Color.white;

            return material;
        }
    }
}