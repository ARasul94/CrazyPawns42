using UniRx;
using UnityEngine;

namespace ViewModels
{
    public class BoardViewModel
    {
        public ReadOnlyReactiveProperty<int> size { get; }
        public ReadOnlyReactiveProperty<Color> blackCellColor { get; }
        public ReadOnlyReactiveProperty<Color> whiteCellColor { get; }
        
        private readonly ReactiveProperty<int> m_size;
        private readonly ReactiveProperty<Color> m_blackCellColor;
        private readonly ReactiveProperty<Color> m_whiteCellColor;

        public BoardViewModel(CrazyPawn.CrazyPawnSettings _settings)
        {
            m_size = new ReactiveProperty<int>(_settings.CheckerboardSize);
            m_blackCellColor = new ReactiveProperty<Color>(_settings.BlackCellColor);
            m_whiteCellColor = new ReactiveProperty<Color>(_settings.WhiteCellColor);
            
            size = new ReadOnlyReactiveProperty<int>(m_size);
            blackCellColor = new ReadOnlyReactiveProperty<Color>(m_blackCellColor);
            whiteCellColor = new ReadOnlyReactiveProperty<Color>(m_whiteCellColor);
        }
    }
}