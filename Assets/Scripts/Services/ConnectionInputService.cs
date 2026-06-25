using Services.ConnectionPreviewService;
using Services.ConnectionSelectionService;
using Services.ConnectionService;
using Services.InputRaycastService;
using Services.InteractionService;
using Services.PawnRegistryService;
using UnityEngine;
using Views;
using Zenject;

namespace Services
{
    public class ConnectionInputService : ITickable, IConnectionSelectionService
    {
        private const float DRAG_THRESHOLD = 8f;

        private readonly IInputRaycastService m_inputRaycastService;
        private readonly IConnectionService m_connectionService;
        private readonly IConnectionPreviewService m_connectionPreviewService;
        private readonly IPawnRegistryService m_pawnRegistry;
        private readonly IInteractionStateService m_interactionStateService;
        private readonly CrazyPawn.CrazyPawnSettings m_settings;

        private ConnectorView m_selectedConnector;
        private ConnectorView m_pressedConnector;
        
        private Vector3 m_pressScreenPosition;
        private bool m_hasPendingSecondClick;

        public ConnectionInputService(
            IInputRaycastService _inputRaycastService,
            IConnectionService _connectionService,
            IConnectionPreviewService _connectionPreviewService,
            IPawnRegistryService _pawnRegistry,
            IInteractionStateService _interactionStateService,
            CrazyPawn.CrazyPawnSettings _settings)
        {
            m_inputRaycastService = _inputRaycastService;
            m_connectionService = _connectionService;
            m_connectionPreviewService = _connectionPreviewService;
            m_pawnRegistry = _pawnRegistry;
            m_interactionStateService =  _interactionStateService;
            m_settings = _settings;
        }

        public void Tick()
        {
            if (Input.GetMouseButtonDown(0))
                OnMouseDown();

            if (m_pressedConnector != null && Input.GetMouseButton(0))
                UpdateConnectionDragState();
            
            if (m_selectedConnector != null)
                m_connectionPreviewService.Update();

            if (Input.GetMouseButtonUp(0))
                OnMouseUp();

            m_connectionService.UpdateLines();
        }
        
        public void CancelSelectionIfRelatedTo(PawnView _pawn)
        {
            if (_pawn == null)
                return;

            var selectedOwner = m_selectedConnector != null ? m_selectedConnector.owner : null;
            var pressedOwner = m_pressedConnector != null ? m_pressedConnector.owner : null;

            if (selectedOwner != _pawn && pressedOwner != _pawn)
                return;

            CancelSelection();
        }

        public void CancelSelection()
        {
            if (m_interactionStateService.isConnectionDragging)
                m_interactionStateService.End(InteractionMode.CONNECTION_DRAG);

            ClearSelection();
        }

        private void OnMouseDown()
        {
            m_pressScreenPosition = Input.mousePosition;
            
            var connector = TryGetConnectorUnderMouse();

            if (m_selectedConnector != null)
            {
                m_hasPendingSecondClick = true;
                return;
            }

            if (connector == null)
                return;
            
            if (!m_interactionStateService.isFree)
                return;

            StartConnectionSelection(connector);
        }
        
        private void StartConnectionSelection(ConnectorView _connector)
        {
            m_selectedConnector = _connector;
            m_pressedConnector = _connector;

            HighlightAvailableConnectors(_connector);
            m_connectionPreviewService.Show(_connector);
        }
        
        private void UpdateConnectionDragState()
        {
            if (m_interactionStateService.isConnectionDragging)
                return;

            var distance = Vector3.Distance(m_pressScreenPosition, Input.mousePosition);

            if (distance < DRAG_THRESHOLD)
                return;

            m_interactionStateService.TryBegin(InteractionMode.CONNECTION_DRAG);
        }

        private void OnMouseUp()
        {
            if (m_interactionStateService.isConnectionDragging)
            {
                FinishConnectionDrag();
                return;
            }
            
            if (m_hasPendingSecondClick)
            {
                FinishSecondClick();
                return;
            }

            m_pressedConnector = null;
        }
        
        private void FinishConnectionDrag()
        {
            var targetConnector = TryGetConnectorUnderMouse();

            m_connectionService.TryCreateConnection(m_pressedConnector, targetConnector);
            
            ClearSelection();
            m_interactionStateService.End(InteractionMode.CONNECTION_DRAG);
        }
        
        private void FinishSecondClick()
        {
            m_hasPendingSecondClick = false;

            var isRealDrag = IsPointerMovedEnoughForDrag();

            if (isRealDrag && (m_interactionStateService.isPawnDragging || m_interactionStateService.isCameraPanning))
                return;

            var targetConnector = TryGetConnectorUnderMouse();

            if (targetConnector != null)
                m_connectionService.TryCreateConnection(m_selectedConnector, targetConnector);

            ClearSelection();
        }
        
        private bool IsPointerMovedEnoughForDrag()
        {
            var distance = Vector3.Distance(m_pressScreenPosition, Input.mousePosition);
            return distance >= DRAG_THRESHOLD;
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
            m_connectionPreviewService.Hide();
            
            foreach (var pawn in m_pawnRegistry.pawns)
            {
                foreach (var connector in pawn.connectors)
                    connector.RestoreMaterial();
            }

            m_selectedConnector = null;
            m_pressedConnector = null;
            m_hasPendingSecondClick = false;
        }
    }
}