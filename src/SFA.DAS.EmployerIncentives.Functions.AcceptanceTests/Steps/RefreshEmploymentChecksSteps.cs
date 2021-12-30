using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature= "RefreshEmploymentChecks")]
    public class RefreshEmploymentChecksSteps : StepsBase
    {
        private readonly TestContext _testContext;

        public RefreshEmploymentChecksSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
        }

        [When(@"an employment checks refresh is triggered")]
        public async Task WhenAnEmploymentChecksRefreshJobIsTriggered()
        {
            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                    Request
                        .Create()
                        .WithPath(x => x.Contains("jobs"))
                        .UsingPut())
                .RespondWith(
                    Response.Create(new ResponseMessage())
                        .WithStatusCode(HttpStatusCode.OK));

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/HttpTriggerRefreshEmploymentChecks")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };
            await _testContext.LegalEntitiesFunctions.HttpTriggerHandleRefreshEmploymentChecks.RunHttp(request);
        }

        [Then(@"the Employer Incentives API is called to update employment checks for apprenticeships")]
        public void ThenTheEmployerIncentivesAPIIsCalledToUpdateEmploymentChecksForApprenticeships()
        {
            var requests = _testContext
                .EmployerIncentivesApi
                .MockServer
                .FindLogEntries(
                    Request
                        .Create()
                        .WithPath(x => x.Contains("/jobs"))
                        .UsingPut()).AsEnumerable();

            requests.Should().HaveCount(1, "Expected request to APIM was not found in Mock server logs");
        }

    }
}
