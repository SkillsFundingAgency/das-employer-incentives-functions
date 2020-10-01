using FluentAssertions;
using Moq;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Infrastructure.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Steps
{
    [Binding]
    public class VendorRegistrationFormSteps : StepsBase
    {
        private readonly TestContext _testContext;
        private readonly DateTime _fakeCurrentDateTime = DateTime.SpecifyKind(new DateTime(2020, 9, 1, 2, 3, 4), DateTimeKind.Utc);
        private readonly IVrfCaseRefreshRepository _repository = new VrfCaseRefreshRepository("UseDevelopmentStorage=true", "LOCAL");

        public VendorRegistrationFormSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _testContext.LegalEntitiesFunctions.MockDateTimeProvider.Setup(x => x.GetCurrentDateTime()).ReturnsAsync(_fakeCurrentDateTime);
        }

        [When(@"a VRF case status update job is triggered")]
        public async Task WhenARequestToUpdateVrfCaseStatusesForLegalEntitiesIsReceived()
        {
            var lastRunDate = await _repository.GetLastRunDateTime();

            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                    Request
                        .Create()
                        .WithPath("/legalentities/vendorregistrationform/status")
                        .WithParam("from", $"{lastRunDate.ToIsoDateTime()}")
                        .WithParam("to", $"{_fakeCurrentDateTime.ToIsoDateTime()}")
                        .UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.NoContent)
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
                        .WithPath(x => x.Contains("/legalentities/vendorregistrationform/status"))
                        .WithParam("from")
                        .WithParam("to")
                        .UsingPatch()).AsEnumerable();

            requests.Should().HaveCount(1, "expected request to APIM was not found in Mock server logs");
        }

        [Then(@"last job run date time is updated")]
        public async Task ThenLastUpdateDateIsStored()
        {
            var lastRunDate = await _repository.GetLastRunDateTime();

            lastRunDate.Should().BeCloseTo(_fakeCurrentDateTime, 60000 /* 1 minute precision */);
        }

    }
}
