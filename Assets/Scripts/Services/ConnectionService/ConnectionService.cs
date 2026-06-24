using System.Collections.Generic;
using Factories.ConnectionLineFactory;
using Models;
using Views;

namespace Services.ConnectionService
{
    public class ConnectionService : IConnectionService
    {
        private readonly IConnectionLineFactory m_lineFactory;
        private readonly List<ConnectionModel> m_connections = new();
        private readonly List<ConnectionLineView> m_lineViews = new();

        public ConnectionService(IConnectionLineFactory _lineFactory)
        {
            m_lineFactory = _lineFactory;
        }

        public bool CanConnect(ConnectorView _from, ConnectorView _to)
        {
            if (_from == null || _to == null)
                return false;

            if (_from == _to)
                return false;

            if (_from.owner == null || _to.owner == null)
                return false;

            if (_from.owner == _to.owner)
                return false;

            return true;
        }

        bool IConnectionService.TryCreateConnection(ConnectorView _from, ConnectorView _to)
        {
            return TryCreateConnection(_from, _to);
        }

        void IConnectionService.RemoveConnectionsOf(PawnView _pawn)
        {
            RemoveConnectionsOf(_pawn);
        }

        public bool TryCreateConnection(ConnectorView _from, ConnectorView _to)
        {
            if (!CanConnect(_from, _to))
                return false;

            if (HasConnection(_from, _to))
                return false;

            var model = new ConnectionModel(_from, _to);
            var view = m_lineFactory.Create(model);

            m_connections.Add(model);
            m_lineViews.Add(view);

            return true;
        }

        public void RemoveConnectionsOf(PawnView _pawn)
        {
            for (var i = m_connections.Count - 1; i >= 0; i--)
            {
                if (!m_connections[i].Contains(_pawn))
                    continue;

                m_connections.RemoveAt(i);

                m_lineViews[i].DestroyView();
                m_lineViews.RemoveAt(i);
            }
        }

        public void UpdateLines()
        {
            foreach (var lineView in m_lineViews)
                lineView.UpdatePositions();
        }

        private bool HasConnection(ConnectorView _from, ConnectorView _to)
        {
            foreach (var connection in m_connections)
            {
                if (connection.Matches(_from, _to))
                    return true;
            }

            return false;
        }
    }
}