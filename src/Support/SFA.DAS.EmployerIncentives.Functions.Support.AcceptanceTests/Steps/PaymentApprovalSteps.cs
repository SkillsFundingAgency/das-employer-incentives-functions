using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs.Types;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.Support.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "PaymentApproval")]
    public class PaymentApprovalSteps : StepsBase    
    {
        private readonly TestContext _testContext;
        
        public PaymentApprovalSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
        }

        [When(@"a request to trigger payment approval is received")]
        public async Task WhenTriggerPaymentApprovalRequestIsReceived()
        {
            var jobRequest = _testContext.TestData.GetOrCreate(onCreate: () =>
            {
                return new JobRequest
                {
                    Type = JobType.PaymentApproval,
                    Data = new Dictionary<string, string>()
                };
            });

            _testContext.EmployerIncentivesApi.MockServer
               .Given(
                       Request
                       .Create()
                       .WithPath(x => x.Contains("jobs"))
                       .UsingPut()
                       )
                   .RespondWith(
               Response.Create()
                   .WithStatusCode(HttpStatusCode.OK)
                   .WithHeader("Content-Type", "application/json"));

            await _testContext.LegalEntitiesFunctions.HttpTriggerHandleTriggerPaymentApproval.RunHttp(null);
        }
        
        [Then(@"the request is forwarded to the Employer Incentives system")]
        public void ThenTheRefreshRequestIsForwardedToTheApi()
        {
            var jobRequest = _testContext.TestData.GetOrCreate<JobRequest>();

            var requests = _testContext
                       .EmployerIncentivesApi
                       .MockServer
                       .FindLogEntries(
                           Request
                           .Create()
                           .WithPath(x => x.Contains("jobs")));

            requests.AsEnumerable().Count().Should().Be(1);
        }
    }    
}
