using System;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Monkey.SimpleInjector;
using Monkey.WebApi.SimpleInjector;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using TechTalk.SpecFlow;

namespace Monkey.WebApi.AcceptanceTests.Configuration
{
    [Binding]
    public class ApplicationUnderTests
    {
        private ScenarioContext _context;
        private Container _container;
        private IApplicationExecutor _executor;
        private IWebHost _webHost;
        public IApplicationExecutor Executor => _executor;

        public ApplicationUnderTests(ScenarioContext context)
        {
            _context = context;
        }


        [BeforeScenario()]
        public void InitContainer()
        {
            _webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();

            _webHost.Start();
            _container = (Container)_webHost.Services.GetService(typeof(IServiceProvider));
            _executor = ApplicationExecutor.Create(_container);

            _context.ScenarioContainer.RegisterInstanceAs(Executor);
        }
    }
}