using TechTalk.SpecFlow;

namespace Monkey.WebApi.AcceptanceTests.Configuration
{
    [Binding]
    public class ApplicationUnderTestsSteps
    {
        private IApplicationExecutor _applicationExecutor;

        public ApplicationUnderTestsSteps(IApplicationExecutor applicationExecutor)
        {
            _applicationExecutor = applicationExecutor;
        }


        [Given(@"The applications container is configured")]
        public void GivenTheApplicationsContainerIsConfigured()
        {
            // it's done though hooks
        }



    }
}
