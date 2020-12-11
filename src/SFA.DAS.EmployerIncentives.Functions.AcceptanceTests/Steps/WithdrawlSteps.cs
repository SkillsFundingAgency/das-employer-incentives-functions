using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawls.Types;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "Withdrawl")]
    public class WithdrawlSteps : StepsBase    
    {
        private readonly TestContext _testContext;
        private readonly Fixture _fixture;
        private WithdrawRequest _withDrawRequest;

        public WithdrawlSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _fixture = new Fixture();
            _withDrawRequest = new WithdrawRequest
            {
                Type = WithdrawlType.Employer,
                AccountLegalEntityId = _fixture.Create<long>(),
                ULN = _fixture.Create<long>(),
                ServiceRequest = _fixture.Create<ServiceRequest>()
            };
        }

        [When(@"a withdrawl request is received")]
        public async Task WhenAWithdrawlRequestIsReceived()
        {
            var json = JsonConvert.SerializeObject(_withDrawRequest);

            _testContext.EmployerIncentivesApi.MockServer
               .Given(
                       Request
                       .Create()
                       .WithPath($"/api/withdrawls")
                       .WithBody(json)
                       .UsingPost()
                       )
                   .RespondWith(
               Response.Create()
                   .WithStatusCode(HttpStatusCode.OK)
                   .WithHeader("Content-Type", "application/json"));

            var request = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            await _testContext.LegalEntitiesFunctions.HttpTriggerHandleWithdrawl.RunHttp(request);
        }


        [Then(@"the withdrawl request is forwarded to the Employer Incentives API")]
        public void ThenTheEventIsForwardedToTheApi()
        {
            var requests = _testContext
                       .EmployerIncentivesApi
                       .MockServer
                       .FindLogEntries(
                           Request
                           .Create()
                           .WithPath($"/api/withdrawls")
                           .WithBody(JsonConvert.SerializeObject(_withDrawRequest))
                           .UsingPost());

            requests.AsEnumerable().Count().Should().Be(1);
        }
    }    
}
