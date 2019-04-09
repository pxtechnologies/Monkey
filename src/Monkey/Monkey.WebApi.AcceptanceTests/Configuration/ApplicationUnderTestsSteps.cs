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


        [Given(@"I have writen my model with IRequestHandler pattern")]
        public void GivenIHaveWritenMyModelWithIRequestHandlerPattern()
        {
            // Nothing to do here
        }
        [Given(@"I configured basic WebApi features with swagger")]
        public void GivenIConfiguredBasicWebApiFeaturesWithSwagger()
        {
            // 
        }
        [Given(@"I add dynamic api to mvc")]
        public void GivenIAddDynamicApiToMvc()
        {
            
        }


    }
}
