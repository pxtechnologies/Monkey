using System.Reflection;
using Monkey.Sql.SimpleInjector;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using TechTalk.SpecFlow;

namespace Monkey.Sql.AcceptanceTests.Configuration
{
    [Binding]
    public class ApplicationUnderTests
    {
        private ScenarioContext _context;
        private Container _container;
        private IApplicationExecutor _executor;

        public IApplicationExecutor Executor => _executor;

        public ApplicationUnderTests(ScenarioContext context)
        {
            _context = context;
        }


        [BeforeScenario()]
        public void InitContainer()
        {
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            _container.Options.DefaultLifestyle = Lifestyle.Scoped;

            Assembly[] catalog = new Assembly[]
            {
                typeof(ApplicationUnderTestsPackage).Assembly,
                typeof(SqlPackage).Assembly
            };
            _container.RegisterPackages(catalog);
            _container.Verify();

            _executor = ApplicationExecutor.Create(_container);

            _context.ScenarioContainer.RegisterInstanceAs(Executor);
        }
    }
}