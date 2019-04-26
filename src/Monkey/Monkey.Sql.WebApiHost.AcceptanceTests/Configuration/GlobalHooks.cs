using Gherkin.Events.Args.Ast;
using Microsoft.Extensions.Configuration;
using TechTalk.SpecFlow;

namespace Monkey.Sql.WebApiHost.AcceptanceTests.Configuration
{
    [Binding]
    public class GlobalHooks
    {
        private readonly ScenarioContext _context;
        private IApplicationExecutor _executor;
        public GlobalHooks(ScenarioContext context)
        {
            _context = context;
        }

        [BeforeFeature()]
        public static void BeforeFeature()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables()
                .AddJsonFile("appsettings.json");
            IConfiguration config = builder.Build();


            FeatureContext.Current.FeatureContainer.RegisterInstanceAs(config);
        }

        [AfterScenario()]
        public void AfterScenario()
        {

        }

        [BeforeScenario()]
        public void BeforeScenario()
        {
            _executor = ApplicationExecutor.CreateEmpty();
            _context.ScenarioContainer.RegisterInstanceAs(_executor);

            
        }
    }
}