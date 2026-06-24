using UniRx;
using UnityEngine;

namespace ViewModels
{
    public class BoardViewModel
    {
        public ReactiveProperty<int> size { get; }
        public ReactiveProperty<Color> blackCellColor { get; }
        public ReactiveProperty<Color> whiteCellColor { get; }

        public BoardViewModel(CrazyPawn.CrazyPawnSettings _settings)
        {
            size = new ReactiveProperty<int>(_settings.CheckerboardSize);
            blackCellColor = new ReactiveProperty<Color>(_settings.BlackCellColor);
            whiteCellColor = new ReactiveProperty<Color>(_settings.WhiteCellColor);
        }
    }
}