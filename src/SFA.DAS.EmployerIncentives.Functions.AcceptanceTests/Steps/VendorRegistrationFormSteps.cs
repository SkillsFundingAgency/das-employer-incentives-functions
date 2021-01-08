using FluentAssertions;
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

        public VendorRegistrationFormSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
        }

        [When(@"a VRF case status update job is triggered")]
        public async Task WhenARequestToUpdateVrfCaseStatusesForLegalEntitiesIsReceived()
        {
            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                    Request
                        .Create()
                        .WithPath("/api/legalentities/vendorregistrationform")
                        .UsingPatch())
                .RespondWith(
                    Response.Create(new ResponseMessage())
                        .WithStatusCode(HttpStatusCode.OK)
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
                        .WithPath(x => x.Contains("legalentities/vendorregistrationform"))
                        .UsingPatch()).AsEnumerable();

            requests.Should().HaveCount(1, "expected request to APIM was not found in Mock server logs");
        }
    }
}
