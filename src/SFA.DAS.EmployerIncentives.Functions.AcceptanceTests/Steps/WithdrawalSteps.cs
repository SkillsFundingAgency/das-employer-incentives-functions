﻿using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals.Types;
using SFA.DAS.EmployerIncentives.Types;
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
    [Scope(Feature = "Withdrawal")]
    public class WithdrawalSteps : StepsBase    
    {
        private readonly TestContext _testContext;
        private readonly Fixture _fixture;
        private readonly WithdrawRequest _withdrawRequest;
        private readonly ReinstateApplicationRequest _reinstateApplicationRequest;

        public WithdrawalSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _fixture = new Fixture();
            _withdrawRequest = new WithdrawRequest
            {
                WithdrawalType = WithdrawalType.Employer,
                Applications = new [] 
                {
                    new Application { AccountLegalEntityId = _fixture.Create<long>(), ULN = _fixture.Create<long>() }
                },
                ServiceRequest = _fixture.Create<ServiceRequest>()
            };

            _reinstateApplicationRequest = _fixture.Create<ReinstateApplicationRequest>();
        }

        [When(@"an employer withdrawal request is received")]
        public async Task WhenAnEmployerWithdrawalRequestIsReceived()
        {
            _withdrawRequest.WithdrawalType = WithdrawalType.Employer;

            await WhenAWithdrawalRequestIsReceived();
        }

        [When(@"a compliance withdrawal request for a single application is received")]
        public async Task WhenAComplianceWithdrawalRequestForASingleApplicationIsReceived()
        {
            _withdrawRequest.WithdrawalType = WithdrawalType.Compliance;

            await WhenAWithdrawalRequestIsReceived();
        }

        [When(@"a compliance withdrawal request with multiple applications is received")]
        public async Task WhenAComplianceWithdrawalRequestForMultipleApplicationsIsReceived()
        {
            _withdrawRequest.WithdrawalType = WithdrawalType.Compliance;
            _withdrawRequest.Applications = new []
            {
                new Application { AccountLegalEntityId = _fixture.Create<long>(), ULN = _fixture.Create<long>() },
                new Application { AccountLegalEntityId = _fixture.Create<long>(), ULN = _fixture.Create<long>() },
                new Application { AccountLegalEntityId = _fixture.Create<long>(), ULN = _fixture.Create<long>() }
            };

            await WhenAWithdrawalRequestIsReceived();
        }

        [When(@"a reinstate application request is received")]
        public async Task WhenAReinstateApplicationRequestIsReceived()
        {
            var json = JsonConvert.SerializeObject(_reinstateApplicationRequest);

            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                    Request
                        .Create()
                        .WithPath($"/api/withdrawal-reinstatements")
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
            await _testContext.LegalEntitiesFunctions.HttpTriggerHandleReinstateApplication.RunHttp(request);
        }

        private async Task WhenAWithdrawalRequestIsReceived()
        {
            var json = JsonConvert.SerializeObject(_withdrawRequest);

            _testContext.EmployerIncentivesApi.MockServer
               .Given(
                       Request
                       .Create()
                       .WithPath($"/api/withdrawals")
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
            await _testContext.LegalEntitiesFunctions.HttpTriggerHandleWithdrawal.RunHttp(request);
        }

        [Then(@"the withdrawal request is forwarded to the Employer Incentives API")]
        public void ThenTheEventIsForwardedToTheApi()
        {
            var requests = _testContext
                       .EmployerIncentivesApi
                       .MockServer
                       .FindLogEntries(
                           Request
                           .Create()
                           .WithPath($"/api/withdrawals")
                           .WithBody(JsonConvert.SerializeObject(_withdrawRequest))
                           .UsingPost());

            requests.AsEnumerable().Count().Should().Be(1);
        }

        [Then(@"the reinstate application request is forwarded to the Employer Incentives API")]
        public void ThenTheReinstateEventIsForwardedToTheApi()
        {
            var requests = _testContext
                .EmployerIncentivesApi
                .MockServer
                .FindLogEntries(
                    Request
                        .Create()
                        .WithPath($"/api/withdrawal-reinstatements")
                        .WithBody(JsonConvert.SerializeObject(_reinstateApplicationRequest))
                        .UsingPost());

            requests.AsEnumerable().Count().Should().Be(1);
        }
    }    
}
