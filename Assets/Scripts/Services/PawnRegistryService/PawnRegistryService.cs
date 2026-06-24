using System.Collections.Generic;
using Views;

namespace Services.PawnRegistryService
{
    public class PawnRegistryService: IPawnRegistryService
    {
        private readonly List<PawnView> m_pawns = new();

        public IReadOnlyList<PawnView> pawns => m_pawns;

        public void Register(PawnView _pawn)
        {
            if (_pawn == null || m_pawns.Contains(_pawn))
                return;

            m_pawns.Add(_pawn);
            _pawn.Destroying += Unregister;
        }

        public void Unregister(PawnView _pawn)
        {
            if (_pawn == null)
                return;

            _pawn.Destroying -= Unregister;
            m_pawns.Remove(_pawn);
        }
    }
}