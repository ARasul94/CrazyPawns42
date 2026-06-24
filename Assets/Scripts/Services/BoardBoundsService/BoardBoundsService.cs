using UnityEngine;

namespace Services.BoardBoundsService
{
    public class BoardBoundsService : IBoardBoundsService
    {
        private const float CELL_SIZE = 1.5f;

        private readonly float m_halfBoardSize;

        public BoardBoundsService(CrazyPawn.CrazyPawnSettings _settings)
        {
            m_halfBoardSize = _settings.CheckerboardSize * CELL_SIZE * 0.5f;
        }

        public bool Contains(Vector3 _pawnCenterPosition)
        {
            return _pawnCenterPosition.x >= -m_halfBoardSize &&
                   _pawnCenterPosition.x <= m_halfBoardSize &&
                   _pawnCenterPosition.z >= -m_halfBoardSize &&
                   _pawnCenterPosition.z <= m_halfBoardSize;
        }
    }
}