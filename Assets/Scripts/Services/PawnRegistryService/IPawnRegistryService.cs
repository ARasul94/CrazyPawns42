using System.Collections.Generic;
using Views;

namespace Services.PawnRegistryService
{
    public interface IPawnRegistryService
    {
        IReadOnlyList<PawnView> pawns { get; }

        void Register(PawnView _pawn);
        void Unregister(PawnView _pawn);
    }
}