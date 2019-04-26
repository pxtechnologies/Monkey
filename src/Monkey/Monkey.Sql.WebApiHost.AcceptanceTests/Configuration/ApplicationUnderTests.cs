using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Monkey.Sql.WebApiHost.Services;
using SimpleInjector;
using TechTalk.SpecFlow;

namespace Monkey.Sql.WebApiHost.AcceptanceTests.Configuration
{
    [Binding]
    public class ApplicationUnderTests
    {
        private Container _container;
        private IApplicationExecutor _executor;
        private IWebHost _webHost;
        public IApplicationExecutor Executor => _executor;

        public ApplicationUnderTests(IApplicationExecutor exe)
        {
            _executor = exe;
        }

        [AfterScenario()]
        public void CleanUp()
        {
            if (_webHost != null)
            {
               
                _webHost.StopAsync().GetAwaiter().GetResult();
                
                _executor.Execute<ContainerAccessor>(c => c.Container.Dispose());
                _webHost.Dispose();
            }
        }

        
        public void Run()
        {
            _webHost = Program.CreateWebHostBuilder(new string[0])
                .Build();
            
            _webHost.Start();
            _container = ((ContainerAccessor)_webHost.Services.GetService(typeof(ContainerAccessor))).Container;
            _executor.BindContainer(_container);
        }
    }
}