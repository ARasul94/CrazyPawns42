using Services.BoardBoundsService;
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
    public class PawnDragService : ITickable
    {
        private readonly IInputRaycastService m_inputRaycastService;
        private readonly IBoardBoundsService m_boardBoundsService;
        private readonly IConnectionService m_connectionService;
        private readonly IPawnRegistryService m_pawnRegistryService;
        private readonly IInteractionStateService m_interactionStateService;
        private readonly IConnectionSelectionService m_connectionSelectionService;

        private PawnView m_draggedPawn;
        private Vector3 m_dragOffset;

        public PawnDragService(
            IInputRaycastService _inputRaycastService,
            IBoardBoundsService _boardBoundsService,
            IConnectionService _connectionService,
            IPawnRegistryService _pawnRegistryService,
            IInteractionStateService _interactionStateService,
            IConnectionSelectionService _connectionSelectionService)
        {
            m_inputRaycastService = _inputRaycastService;
            m_boardBoundsService = _boardBoundsService;
            m_connectionService =  _connectionService;
            m_pawnRegistryService = _pawnRegistryService;
            m_interactionStateService = _interactionStateService;
            m_connectionSelectionService =  _connectionSelectionService;
        }

        public void Tick()
        {
            if (Input.GetMouseButtonDown(0))
                TryStartDrag();

            if (m_draggedPawn != null && Input.GetMouseButton(0))
                UpdateDrag();

            if (m_draggedPawn != null && Input.GetMouseButtonUp(0))
                FinishDrag();
        }

        private void TryStartDrag()
        {
            if (!m_interactionStateService.isFree)
                return;
            
            if (!m_inputRaycastService.RaycastFromMouse(out var hit))
                return;

            var pawn = hit.collider.GetComponentInParent<PawnView>();

            if (pawn == null || !pawn.IsBodyCollider(hit.collider))
                return;

            if (!m_inputRaycastService.TryGetMousePointOnGround(out var groundPoint))
                return;
            
            if (!m_interactionStateService.TryBegin(InteractionMode.PAWN_DRAG))
                return;

            m_draggedPawn = pawn;
            m_dragOffset = pawn.cachedTransform.position - groundPoint;
        }

        private void UpdateDrag()
        {
            if (!m_inputRaycastService.TryGetMousePointOnGround(out var groundPoint))
                return;

            var targetPosition = groundPoint + m_dragOffset;
            targetPosition.y = 0f;

            m_draggedPawn.viewModel.SetPosition(targetPosition);
            m_draggedPawn.viewModel.SetIsOutsideBoard(!m_boardBoundsService.Contains(targetPosition));
        }

        private void FinishDrag()
        {
            var shouldDelete = m_draggedPawn.viewModel.isOutsideBoard.Value;
            var pawnToRelease = m_draggedPawn;
            
            m_interactionStateService.End(InteractionMode.PAWN_DRAG);

            m_draggedPawn = null;

            if (shouldDelete)
            {
                m_connectionSelectionService.CancelSelectionIfRelatedTo(pawnToRelease);
                m_connectionService.RemoveConnectionsOf(pawnToRelease);
                m_pawnRegistryService.Unregister(pawnToRelease);
                pawnToRelease.DestroyView();
            }
        }
    }
}