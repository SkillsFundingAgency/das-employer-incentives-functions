using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Payments.Types;
using SFA.DAS.EmployerIncentives.Types;
using TechTalk.SpecFlow;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.Support.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "RevertPayments")]
    public class RevertPaymentsSteps : StepsBase
    {
        private readonly TestContext _testContext;
        private readonly Fixture _fixture;
        private readonly RevertPaymentsRequest _revertPaymentsRequest;
        private IActionResult _response;

        public RevertPaymentsSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _fixture = new Fixture();
            _revertPaymentsRequest = new RevertPaymentsRequest
            {
                Payments = _fixture.CreateMany<Guid>(10).ToList(),
                ServiceRequest = _fixture.Create<ServiceRequest>()
            };
        }

        [When(@"a revert payments request is received")]
        public async Task WhenARevertPaymentsRequestIsReceived()
        {
            var json = JsonConvert.SerializeObject(_revertPaymentsRequest);

            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                    Request
                        .Create()
                        .WithPath($"/api/revert-payments")
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
            _response = await _testContext.LegalEntitiesFunctions.HttpTriggerHandleRevertPaymentsRequest.RunHttp(request);
        }

        [When(@"a revert request is received but no matching payment is found")]
        public async Task WhenARevertPaymentsRequestIsReceivedButNoMatchingPaymentIsFound()
        {
            var json = JsonConvert.SerializeObject(_revertPaymentsRequest);

            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                    Request
                        .Create()
                        .WithPath($"/api/revert-payments")
                        .WithBody(json)
                        .UsingPost()
                )
                .RespondWith(
                    Response.Create(new ResponseMessage())
                        .WithStatusCode(HttpStatusCode.BadRequest)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody($"Payment Id {_revertPaymentsRequest.Payments[0]} not found"));

            var request = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            _response = await _testContext.LegalEntitiesFunctions.HttpTriggerHandleRevertPaymentsRequest.RunHttp(request);
        }


        [Then(@"the revert payments request is forwarded to the Employer Incentives API")]
        public void ThenTheRequestIsForwardedToTheAPI()
        {
            var requests = _testContext
                .EmployerIncentivesApi
                .MockServer
                .FindLogEntries(
                    Request
                        .Create()
                        .WithPath($"/api/revert-payments")
                        .WithBody(JsonConvert.SerializeObject(_revertPaymentsRequest))
                        .UsingPost());

            requests.AsEnumerable().Count().Should().Be(1);
        }

        [Then(@"the user receives a Bad Request response")]
        public void ThenTheUserReceivesABadRequestResponse()
        {
            var result = (_response as ContentResult);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            result.Content.Contains($"Payment Id {_revertPaymentsRequest.Payments[0]} not found");
        }
    }
}
