using Models;
using Views;

namespace Factories.ConnectionLineFactory
{
    public interface IConnectionLineFactory
    {
        ConnectionLineView Create(ConnectionModel _model);
    }
}