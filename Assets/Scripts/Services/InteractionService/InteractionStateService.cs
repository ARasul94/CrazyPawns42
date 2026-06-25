namespace Services.InteractionService
{
    public class InteractionStateService : IInteractionStateService
    {
        public InteractionMode currentMode { get; private set; } = InteractionMode.NONE;

        public bool isFree => currentMode == InteractionMode.NONE;
        public bool isPawnDragging => currentMode == InteractionMode.PAWN_DRAG;
        public bool isConnectionDragging => currentMode == InteractionMode.CONNECTION_DRAG;
        public bool isCameraPanning => currentMode == InteractionMode.CAMERA_PAN;

        public bool TryBegin(InteractionMode _mode)
        {
            if (!isFree)
                return false;

            currentMode = _mode;
            return true;
        }

        public void End(InteractionMode _mode)
        {
            if (currentMode != _mode)
                return;

            currentMode = InteractionMode.NONE;
        }

        public void ForceReset()
        {
            currentMode = InteractionMode.NONE;
        }
    }
}