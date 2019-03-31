using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Monkey.Sql.SimpleInjector;
using TechTalk.SpecFlow;

namespace Monkey.Sql.AcceptanceTests.Configuration
{
    [Binding]
    public class ApplicationUnderTestsSteps
    {
        private IApplicationExecutor _applicationExecutor;

        public ApplicationUnderTestsSteps(IApplicationExecutor applicationExecutor)
        {
            _applicationExecutor = applicationExecutor;
        }

        [Given(@"I have my system configured with SqlServer")]
        public async Task GivenIHaveMySystemConfiguredWithSqlServer()
        {
            await _applicationExecutor.ExecuteAsync<IMonkeyDatabase>(async x=> await x.ReCreate());
        }
        [Given(@"I configured basic WebApi features with swagger")]
        public void GivenIConfiguredBasicWebApiFeaturesWithSwagger()
        {
            
        }

    }
}
