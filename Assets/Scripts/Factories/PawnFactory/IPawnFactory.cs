using UnityEngine;
using Views;

namespace Factories.PawnFactory
{
    public interface IPawnFactory
    {
        PawnView Create(Vector3 _position);
    }
}