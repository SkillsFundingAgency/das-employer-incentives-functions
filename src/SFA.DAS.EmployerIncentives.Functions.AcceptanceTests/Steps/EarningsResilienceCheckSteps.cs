using FluentAssertions;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "EarningsResilienceCheck")]
    public class EarningsResilienceCheckSteps : StepsBase
    {
        private readonly TestContext _testContext;

        public EarningsResilienceCheckSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
        }

        [When(@"an earnings resilience check is triggered")]
        public async Task WhenAnEarningsResilienceCheckJobIsTriggered()
        {
            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                    Request
                        .Create()
                        .WithPath("/earnings-resilience-check")
                        .UsingPost())
                .RespondWith(
                    Response.Create(new ResponseMessage())
                        .WithStatusCode(HttpStatusCode.OK));

            await _testContext.LegalEntitiesFunctions.TimerTriggerEarningsResilienceCheck.RunTimer(null, new TestLogger());
        }

        [Then(@"the Employer Incentives API is called to update apprenticeships")]
        public void ThenTheEmployerIncentivesAPIIsCalledToUpdateApprenticeships()
        {
            var requests = _testContext
                .EmployerIncentivesApi
                .MockServer
                .FindLogEntries(
                    Request
                        .Create()
                        .WithPath(x => x.Contains("/earnings-resilience-check"))
                        .UsingPost()).AsEnumerable();

            requests.Should().HaveCount(1, "Expected request to APIM was not found in Mock server logs");
        }

    }
}
