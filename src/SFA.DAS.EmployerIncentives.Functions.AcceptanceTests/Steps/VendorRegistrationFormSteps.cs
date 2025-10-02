using FluentAssertions;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Infrastructure.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        [Given(@"the VRF case status update job is paused")]
        public async Task GivenTheVrfCaseStatusUpdateJobIsPaused()
        {
            await _testContext.LegalEntitiesFunctions.RefreshVendorRegistrationCaseStatus.Pause(new HttpRequestMessage(HttpMethod.Post, ""));
            var vrfCaseRefresh = await _repository.Get();
            vrfCaseRefresh.IsPaused.Should().BeTrue();
        }

        [Given(@"the VRF case status update job is not paused")]
        public async Task GivenTheVrfCaseStatusUpdateJobIsNotPaused()
        {
            await _testContext.LegalEntitiesFunctions.RefreshVendorRegistrationCaseStatus.Resume(new HttpRequestMessage(HttpMethod.Post, ""));
            var vrfCaseRefresh = await _repository.Get();
            vrfCaseRefresh.IsPaused.Should().BeFalse();
        }

        [When(@"the VRF case status update job is resumed")]
        public async Task WhenTheVrfCaseStatusUpdateJobIsResumed()
        {
            await _testContext.LegalEntitiesFunctions.RefreshVendorRegistrationCaseStatus.Resume(new HttpRequestMessage(HttpMethod.Post, ""));
        }

        [When(@"a VRF case status update job is triggered")]
        public async Task WhenARequestToUpdateVrfCaseStatusesForLegalEntitiesIsReceived()
        {
            var lastRunDate = (await _repository.Get()).LastRunDateTime;

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

            await _testContext.LegalEntitiesFunctions.RefreshVendorRegistrationCaseStatus.Run(null);
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

        [Then(@"the Employer Incentives API is not called to update Legal Entities")]
        public void ThenTheEventTheEmployerIncentivesSystemIsNotCalled()
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

            requests.Should().HaveCount(0, "expected request to APIM was not found in Mock server logs");
        }

        [Then(@"last job run date time is updated")]
        public async Task ThenLastUpdateDateIsStored()
        {
            var vrfCaseRefresh = await _repository.Get();

            vrfCaseRefresh.LastRunDateTime.Should().Be(DateTime.Parse(_lastCaseUpdateDateTime));
        }

        [Then(@"last job run date time is not updated")]
        public async Task ThenLastUpdateDateIsNotUpdated()
        {
            var vrfCaseRefresh = await _repository.Get();

            vrfCaseRefresh.LastRunDateTime.Should().Be(DateTime.Parse(_lastCaseUpdateDateTime));
        }

        [Then(@"last job run is paused")]
        public async Task ThenLastUpdateDateIsPaused()
        {
            var vrfCaseRefresh = await _repository.Get();

            vrfCaseRefresh.IsPaused.Should().BeTrue();
        }

        [Then(@"last job run is not paused")]
        public async Task ThenLastUpdateDateIsNotPaused()
        {
            var vrfCaseRefresh = await _repository.Get();

            vrfCaseRefresh.IsPaused.Should().BeFalse();
        }
    }
}
