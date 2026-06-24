using UnityEngine;

namespace Services.InputRaycastService
{
    public interface IInputRaycastService
    {
        bool RaycastFromMouse(out RaycastHit _hit);
        bool TryGetMousePointOnGround(out Vector3 _point);
    }
}