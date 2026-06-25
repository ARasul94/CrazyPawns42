using Views;

namespace Services.ConnectionPreviewService
{
    public interface IConnectionPreviewService
    {
        bool isActive { get; }

        void Show(ConnectorView _from);
        void Update();
        void Hide();
    }
}