using Views;

namespace Services.ConnectionSelectionService
{
    public interface IConnectionSelectionService
    {
        void CancelSelectionIfRelatedTo(PawnView _pawn);
        void CancelSelection();
    }
}