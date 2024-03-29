﻿using FluentAssertions;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Infrastructure.Extensions;
using System;
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
    [Scope(Feature = "VendorRegistrationForm")]
    public class VendorRegistrationFormSteps : StepsBase
    {
        private readonly TestContext _testContext;
        private readonly string _lastCaseUpdateDateTime = "2020-10-16T10:00:00";
        private IVrfCaseRefreshRepository _repository;

        public VendorRegistrationFormSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _repository = testContext.LegalEntitiesFunctions.VrfCaseRefreshRepository;
        }

        [When(@"a VRF case status update job is triggered")]
        public async Task WhenARequestToUpdateVrfCaseStatusesForLegalEntitiesIsReceived()
        {
            var lastRunDate = await _repository.GetLastRunDateTime();

            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                    Request
                        .Create()
                        .WithPath("/api/legalentities/vendorregistrationform/status")
                         .WithParam("from", $"{lastRunDate.ToIsoDateTime()}")
                        .UsingPatch())
                .RespondWith(
                    Response.Create(new ResponseMessage())
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithBody($"\"{_lastCaseUpdateDateTime}\"")
                        .WithHeader("Content-Type", "application/json"));

            await _testContext.LegalEntitiesFunctions.TimerTriggerRefreshVendorRegistrationCaseStatus.Run(null);
        }

        [Then(@"the Employer Incentives API is called to update Legal Entities")]
        public void ThenTheEventTheEmployerIncentivesSystemIsCalled()
        {
            var requests = _testContext
                .EmployerIncentivesApi
                .MockServer
                .FindLogEntries(
                    Request
                        .Create()
                        .WithPath(x => x.Contains("legalentities/vendorregistrationform/status"))
                        .WithParam("from")
                        .UsingPatch()).AsEnumerable();

            requests.Should().HaveCount(1, "expected request to APIM was not found in Mock server logs");
        }

        [Then(@"last job run date time is updated")]
        public async Task ThenLastUpdateDateIsStored()
        {
            var lastRunDate = await _repository.GetLastRunDateTime();

            lastRunDate.Should().Be(DateTime.Parse(_lastCaseUpdateDateTime));
        }

    }
}
