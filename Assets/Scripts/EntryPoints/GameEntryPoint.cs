using Services;
using Zenject;

namespace EntryPoints
{
    public class GameEntryPoint : IInitializable
    {
        private readonly PawnSpawnService m_pawnSpawnService;

        public GameEntryPoint(PawnSpawnService _pawnSpawnService)
        {
            m_pawnSpawnService = _pawnSpawnService;
        }

        public void Initialize()
        {
            m_pawnSpawnService.SpawnInitialPawns();
        }
    }
}