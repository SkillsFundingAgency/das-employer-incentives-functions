using FluentAssertions;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NServiceBus.Transport;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Extensions;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Commands;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "EmployerVendorId")]

    public class EmployerVendorIdSteps : StepsBase
    {
        private readonly string _hashedLegalEntityId;
        private readonly TestContext _testContext;

        public EmployerVendorIdSteps(TestContext testContext) : base(testContext)
        {
            _hashedLegalEntityId = "ABCD123";
            _testContext = testContext;
        }

        [When(@"the AddEmployerEmployerVendorId command is triggered")]
        public async Task WhenAddEmployerEmployerVendorIdTriggered()
        {
            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                    Request
                        .Create()
                        .WithPath($"/api/legalentities/{_hashedLegalEntityId}/employervendorid")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.NoContent)
                        .WithHeader("Content-Type", "application/json"));

            await _testContext.WaitFor<MessageContext>(async () =>
                await _testContext.TestMessageBus.Send(new AddEmployerVendorIdCommand { HashedLegalEntityId = _hashedLegalEntityId }));
        }

        [Then(@"the Employer Incentives API is called to update the Legal Entities Employer Vendor Id")]
        public void ThenTheEmployerIncentivesAPIIsCalledToUpdateTheLegalEntitiesEmployerVendorId()
        {
            var requests = _testContext
                .EmployerIncentivesApi
                .MockServer
                .FindLogEntries(
                    Request
                        .Create()
                        .WithPath(x => x.Contains($"legalentities/{_hashedLegalEntityId}/employervendorid"))
                        .UsingPost()).AsEnumerable();

            requests.Should().HaveCount(1, "expected request to APIM was not found in Mock server logs");
        }
    }
}
