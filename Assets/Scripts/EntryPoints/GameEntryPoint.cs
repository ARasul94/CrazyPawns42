using Services;
using Zenject;

namespace EntryPoints
{
    public class GameEntryPoint : IInitializable
    {
        private readonly PawnSpawnService m_pawnSpawnService;
        private readonly BoardSpawnService m_boardSpawnService;

        public GameEntryPoint(PawnSpawnService _pawnSpawnService,
            BoardSpawnService _boardSpawnService)
        {
            m_pawnSpawnService = _pawnSpawnService;
            m_boardSpawnService = _boardSpawnService;
        }

        public void Initialize()
        {
            m_pawnSpawnService.SpawnInitialPawns();
            m_boardSpawnService.InitializeBoard();
        }
    }
}