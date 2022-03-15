using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RecalculateEarnings.Types;
using TechTalk.SpecFlow;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "RecalculateEarnings")]
    public class RecalculateEarningsSteps : StepsBase
    {
        private readonly TestContext _testContext;
        private Fixture _fixture;
        private RecalculateEarningsRequest _recalculateEarningsRequest;

        public RecalculateEarningsSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _fixture = new Fixture();
        }

        [When(@"a recalculate earnings request is triggered")]
        public async Task WhenARecalculateEarningsRequestIsTriggered()
        {
            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                    Request
                        .Create()
                        .WithPath(x => x.Contains("earningsRecalculations"))
                        .UsingPost())
                .RespondWith(
                    Response.Create(new ResponseMessage())
                        .WithStatusCode(HttpStatusCode.NoContent));

            _recalculateEarningsRequest = _fixture.Build<RecalculateEarningsRequest>()
                .With(x => x.IncentiveLearnerIdentifiers, _fixture.CreateMany<IncentiveLearnerIdentifierDto>().ToList())
                .Create();

            var request = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = new StringContent(JsonConvert.SerializeObject(_recalculateEarningsRequest), Encoding.UTF8, "application/json")
            };

            await _testContext.LegalEntitiesFunctions.HttpTriggerHandleRecalculateEarningsRequest.RunHttp(request);
        }

        [Then(@"the Employer Incentives API is called to recalculate earnings for the specified learners")]
        public void ThenTheEmployerIncentivesAPIIscalledToRecalculateEarningsForTheSpecifiedLearners()
        {
            var requests = _testContext
                .EmployerIncentivesApi
                .MockServer
                .FindLogEntries(
                    Request
                        .Create()
                        .WithPath(x => x.Contains("earningsRecalculations"))
                        .UsingPost()).AsEnumerable();

            requests.Should().HaveCount(1, "Expected request to APIM was not found in Mock server logs");
        }
    }
}
