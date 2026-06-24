using UnityEngine;

namespace Services.BoardBoundsService
{
    public interface IBoardBoundsService
    {
        bool Contains(Vector3 _pawnCenterPosition);
    }
}