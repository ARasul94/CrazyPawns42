namespace Services.InteractionService
{
    public interface IInteractionStateService
    {
        InteractionMode currentMode { get; }

        bool isFree { get; }
        bool isPawnDragging { get; }
        bool isConnectionDragging { get; }
        bool isCameraPanning { get; }

        bool TryBegin(InteractionMode _mode);
        void End(InteractionMode _mode);
        void ForceReset();
    }
}