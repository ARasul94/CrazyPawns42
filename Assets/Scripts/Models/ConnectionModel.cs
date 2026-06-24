using Views;

namespace Models
{
    public class ConnectionModel
    {
        public ConnectorView from { get; }
        public ConnectorView to { get; }

        public ConnectionModel(ConnectorView _from, ConnectorView _to)
        {
            from = _from;
            to = _to;
        }

        public bool Contains(PawnView _pawn)
        {
            return from.owner == _pawn || to.owner == _pawn;
        }

        public bool Matches(ConnectorView _a, ConnectorView _b)
        {
            return from == _a && to == _b || from == _b && to == _a;
        }
    }
}