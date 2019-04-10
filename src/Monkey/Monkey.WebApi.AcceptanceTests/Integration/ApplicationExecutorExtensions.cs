using Monkey.WebApi.AcceptanceTests.Assertion;
using Monkey.WebApi.AcceptanceTests.Configuration;

namespace Monkey.WebApi.AcceptanceTests.Integration
{
    public static class ApplicationExecutorExtensions
    {
        public static void InvokeDynamic(this IApplicationExecutor executor, MockHandlerBuilder builder)
        {
            executor.Execute(builder.CreateDynamicAssertType(), assertation =>
            {
                IDynamicMock a = (IDynamicMock)assertation;
                a.Execute().GetAwaiter().GetResult();
            });
        }
    }
}