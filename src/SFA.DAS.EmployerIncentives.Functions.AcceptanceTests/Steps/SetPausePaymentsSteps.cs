using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals.Types;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments.Types;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "SetPausePaymentsStatus")]
    public class SetPausePaymentsSteps : StepsBase    
    {
        private readonly TestContext _testContext;
        private readonly Fixture _fixture;
        private readonly PausePaymentsRequest _pausePaymentRequest;

        public SetPausePaymentsSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _fixture = new Fixture();
            _pausePaymentRequest = new PausePaymentsRequest
            {
                Action = PausePaymentsAction.NotSet,
                AccountLegalEntityId = _fixture.Create<long>(),
                ULN = _fixture.Create<long>(),
                ServiceRequest = _fixture.Create<ServiceRequest>()
            };
        }

        [When(@"a pause payments request is received")]
        public async Task WhenAnEmployerWithdrawalRequestIsReceived()
        {
            _pausePaymentRequest.Action = PausePaymentsAction.Pause;

            await WhenASetPausePaymentsRequestIsReceived();
        }

        [When(@"a resume payments request is received")]
        public async Task WhenAComplianceWithdrawalRequestIsReceived()
        {
            _pausePaymentRequest.Action = PausePaymentsAction.Pause;

            await WhenASetPausePaymentsRequestIsReceived();
        }

        private async Task WhenASetPausePaymentsRequestIsReceived()
        {
            var json = JsonConvert.SerializeObject(_pausePaymentRequest);

            _testContext.EmployerIncentivesApi.MockServer
               .Given(
                       Request
                       .Create()
                       .WithPath($"/api/pause-payments")
                       .WithBody(json)
                       .UsingPost()
                       )
                   .RespondWith(
               Response.Create()
                   .WithStatusCode(HttpStatusCode.OK)
                   .WithHeader("Content-Type", "application/json"));

            var request = new HttpRequestMessage(HttpMethod.Patch, "")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            await _testContext.LegalEntitiesFunctions.HttpTriggerHandlePausePayments.RunHttp(request);
        }

        [Then(@"the set pause payments status request is forwarded to the Employer Incentives API")]
        public void ThenTheEventIsForwardedToTheApi()
        {
            var requests = _testContext
                       .EmployerIncentivesApi
                       .MockServer
                       .FindLogEntries(
                           Request
                           .Create()
                           .WithPath($"/api/pause-payments")
                           .WithBody(JsonConvert.SerializeObject(_pausePaymentRequest))
                           .UsingPost());

            requests.AsEnumerable().Count().Should().Be(1);
        }
    }    
}
