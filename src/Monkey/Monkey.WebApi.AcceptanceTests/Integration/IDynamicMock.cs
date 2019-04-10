using System.Threading.Tasks;

namespace Monkey.WebApi.AcceptanceTests.Integration
{
    public interface IDynamicMock
    {
        Task Execute();
    }
}