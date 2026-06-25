using UnityEngine;

namespace Settings
{
    public class CameraMoveSettings : MonoBehaviour
    {
        [field: SerializeField, Min(0f)]
        public float zoomSpeed { get; private set; } = 3f;

        [field: SerializeField, Min(0f)]
        public float minCameraHeight { get; private set; } = 3f;

        [field: SerializeField, Min(0f)]
        public float maxCameraHeight { get; private set; } = 35f;
    }
}