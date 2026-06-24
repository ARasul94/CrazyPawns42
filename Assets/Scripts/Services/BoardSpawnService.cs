using ViewModels;
using Views;

namespace Services
{
    public class BoardSpawnService
    {
        private readonly BoardView m_boardView;
        private readonly CrazyPawn.CrazyPawnSettings m_settings;

        public BoardSpawnService(
            BoardView _boardView,
            CrazyPawn.CrazyPawnSettings _settings)
        {
            m_boardView = _boardView;
            m_settings = _settings;
        }

        public void InitializeBoard()
        {
            var viewModel = new BoardViewModel(m_settings);
            m_boardView.Initialize(viewModel);
        }
    }
}