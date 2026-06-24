using Services.ConnectionService;
using Services.InputRaycastService;
using Services.PawnRegistryService;
using UnityEngine;
using Views;
using Zenject;

namespace Services
{
    public class ConnectionInputService : ITickable
    {
        private const float DRAG_THRESHOLD = 8f;

        private readonly IInputRaycastService m_inputRaycastService;
        private readonly IConnectionService m_connectionService;
        private readonly IPawnRegistryService m_pawnRegistry;
        private readonly CrazyPawn.CrazyPawnSettings m_settings;

        private ConnectorView m_selectedConnector;
        private ConnectorView m_pressedConnector;
        private Vector3 m_pressScreenPosition;
        private bool m_isDraggingConnection;

        public ConnectionInputService(
            IInputRaycastService _inputRaycastService,
            IConnectionService _connectionService,
            IPawnRegistryService _pawnRegistry,
            CrazyPawn.CrazyPawnSettings _settings)
        {
            m_inputRaycastService = _inputRaycastService;
            m_connectionService = _connectionService;
            m_pawnRegistry = _pawnRegistry;
            m_settings = _settings;
        }

        public void Tick()
        {
            if (Input.GetMouseButtonDown(0))
                OnMouseDown();

            if (m_pressedConnector != null && Input.GetMouseButton(0))
                UpdateDragState();

            if (m_pressedConnector != null && Input.GetMouseButtonUp(0))
                OnMouseUp();

            m_connectionService.UpdateLines();
        }

        private void OnMouseDown()
        {
            var connector = TryGetConnectorUnderMouse();

            if (m_selectedConnector != null)
            {
                if (connector != null)
                    m_connectionService.TryCreateConnection(m_selectedConnector, connector);

                ClearSelection();
                return;
            }

            if (connector == null)
                return;

            m_selectedConnector = connector;
            m_pressedConnector = connector;
            m_pressScreenPosition = Input.mousePosition;
            m_isDraggingConnection = false;

            HighlightAvailableConnectors(connector);
        }

        private void UpdateDragState()
        {
            if (m_isDraggingConnection)
                return;

            var distance = Vector3.Distance(m_pressScreenPosition, Input.mousePosition);

            if (distance >= DRAG_THRESHOLD)
                m_isDraggingConnection = true;
        }

        private void OnMouseUp()
        {
            if (m_isDraggingConnection)
            {
                var targetConnector = TryGetConnectorUnderMouse();
                m_connectionService.TryCreateConnection(m_pressedConnector, targetConnector);
                ClearSelection();
            }

            m_pressedConnector = null;
            m_isDraggingConnection = false;
        }

        private ConnectorView TryGetConnectorUnderMouse()
        {
            if (!m_inputRaycastService.RaycastFromMouse(out var hit))
                return null;

            return hit.collider.GetComponent<ConnectorView>();
        }

        private void HighlightAvailableConnectors(ConnectorView _source)
        {
            foreach (var pawn in m_pawnRegistry.pawns)
            {
                foreach (var connector in pawn.connectors)
                {
                    if (m_connectionService.CanConnect(_source, connector))
                        connector.SetMaterial(m_settings.ActiveConnectorMaterial);
                }
            }
        }

        private void ClearSelection()
        {
            foreach (var pawn in m_pawnRegistry.pawns)
            {
                foreach (var connector in pawn.connectors)
                    connector.RestoreMaterial();
            }

            m_selectedConnector = null;
            m_pressedConnector = null;
            m_isDraggingConnection = false;
        }
    }
}