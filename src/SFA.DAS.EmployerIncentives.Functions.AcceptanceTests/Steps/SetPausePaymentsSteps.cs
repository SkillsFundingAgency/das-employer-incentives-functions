using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments.Types;
using SFA.DAS.EmployerIncentives.Types;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock;
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
        private IActionResult _response;

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
        public async Task WhenAPausePaymentsRequestIsReceived()
        {
            _pausePaymentRequest.Action = PausePaymentsAction.Pause;

            await WhenASetPausePaymentsRequestIsReceived();
        }

        [When(@"a resume payments request is received")]
        public async Task WhenAResumePaymentsRequestIsReceived()
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

            var request = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            _response = await _testContext.LegalEntitiesFunctions.HttpTriggerHandlePausePayments.RunHttp(request);
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

        [When(@"a pause request is recieved but no matching apprenticeship incentive is found")]
        public async Task WhenAPauseRequestIsRecievedButNoMatchingApprenticeshipIncentiveIsFound()
        {
            _pausePaymentRequest.Action = PausePaymentsAction.Pause;
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
                    Response.Create(new ResponseMessage())
                        .WithStatusCode(HttpStatusCode.NotFound)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(@"{""X"":123}"));

            var request = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            _response = await _testContext.LegalEntitiesFunctions.HttpTriggerHandlePausePayments.RunHttp(request);
        }


        [Then(@"the user receives a Not Found response")]
        public void ThenTheUserReceivesANotFoundResponse()
        {
            var result = (_response as ContentResult);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            result.Content.Contains("X");
            result.Content.Contains("123");

        }

    }
}
