using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments.Types;
using SFA.DAS.EmployerIncentives.Types;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "BlockPayments")]
    public class BlockPaymentsSteps : StepsBase
    {
        private readonly List<BlockAccountLegalEntityForPaymentsRequest> _blockPaymentsRequest;
        private readonly Fixture _fixture;
        private readonly TestContext _testContext;
        private IActionResult _response;

        public BlockPaymentsSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _fixture = new Fixture();
            var request = _fixture.Build<BlockAccountLegalEntityForPaymentsRequest>()
                .With(x => x.ServiceRequest, _fixture.Create<ServiceRequest>())
                .With(x => x.VendorBlocks, new List<VendorBlock>
                {
                    _fixture.Build<VendorBlock>()
                        .With(x => x.VendorBlockEndDate, DateTime.Today.AddMonths(1))
                        .Create(),
                    _fixture.Build<VendorBlock>()
                        .With(x => x.VendorBlockEndDate, DateTime.Today.AddMonths(1))
                        .Create(),
                    _fixture.Build<VendorBlock>()
                        .With(x => x.VendorBlockEndDate, DateTime.Today.AddMonths(1))
                        .Create()
                })
                .Create();
            _blockPaymentsRequest = new List<BlockAccountLegalEntityForPaymentsRequest> { request };
        }

        [When(@"a block payments request is received")]
        public async Task WhenABlockPaymentsRequestIsReceived()
        {
            var json = JsonConvert.SerializeObject(_blockPaymentsRequest);

            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                    Request
                        .Create()
                        .WithPath(x => x.Contains("blockedpayments"))
                        .WithBody(json)
                        .UsingPatch()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.NoContent)
                        .WithHeader("Content-Type", "application/json"));

            var request = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            _response = await _testContext.LegalEntitiesFunctions.HttpTriggerHandleBlockPaymentsRequest
                .RunHttp(request);
        }

        [Then(@"the block payments request is forwarded to the Employer Incentives API")]
        public void ThenTheBlockPaymentsRequestIsForwardedToTheEmploymentIncentivesApi()
        {
            var requests = _testContext
                .EmployerIncentivesApi
                .MockServer
                .FindLogEntries(
                    Request
                        .Create()
                        .WithPath(x => x.Contains("blockedpayments"))
                        .WithBody(JsonConvert.SerializeObject(_blockPaymentsRequest))
                        .UsingPatch());

            requests.AsEnumerable().Count().Should().Be(1);
        }

        [Then(@"the user receives an OK response")]
        public void ThenTheUserReceivesAnOkResponse()
        {
            var statusCodeResult = _response as OkResult;
            statusCodeResult.Should().NotBeNull();
        }
    }
}