using Views;

namespace Services.ConnectionService
{
    public interface IConnectionService
    {
        bool CanConnect(ConnectorView _from, ConnectorView _to);
        bool TryCreateConnection(ConnectorView _from, ConnectorView _to);
        void RemoveConnectionsOf(PawnView _pawn);
        void UpdateLines();
    }
}