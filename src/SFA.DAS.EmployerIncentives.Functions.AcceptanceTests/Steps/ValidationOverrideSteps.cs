using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ValidationOverrides.Types;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals.Types;
using SFA.DAS.EmployerIncentives.Types;
using System;
using System.Collections.Generic;
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
    [Scope(Feature = "ValidationOverride")]
    public class ValidationOverrideSteps : StepsBase    
    {
        private readonly TestContext _testContext;
        private readonly Fixture _fixture;
        private readonly List<ValidationOverride> _validationOverrideRequests;

        public ValidationOverrideSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _fixture = new Fixture();
            _validationOverrideRequests = new List<ValidationOverride>()
            {
                new ValidationOverride()
                {
                    AccountLegalEntityId = _fixture.Create<long>(),
                    ULN = _fixture.Create<long>(),
                    ValidationSteps = new List<ValidationStep>()
                    {
                        new ValidationStep() { ValidationType = ValidationType.IsInLearning, ExpiryDate = DateTime.UtcNow.AddDays(10) },
                        new ValidationStep() { ValidationType = ValidationType.HasNoDataLocks, ExpiryDate = DateTime.UtcNow.AddDays(20) }
                    }.ToArray(),
                    ServiceRequest = _fixture.Create<ServiceRequest>()
                },
                new ValidationOverride()
                {
                    AccountLegalEntityId = _fixture.Create<long>(),
                    ULN = _fixture.Create<long>(),
                    ValidationSteps = new List<ValidationStep>()
                    {
                        new ValidationStep() { ValidationType = ValidationType.EmployedBeforeSchemeStarted, ExpiryDate = DateTime.UtcNow.AddDays(30) }
                    }.ToArray(),
                    ServiceRequest = _fixture.Create<ServiceRequest>()
                }
            };
        }

        [When(@"a validation override request is received")]
        public async Task WhenAValidationOverrideRequestIsReceived()
        {
            var json = JsonConvert.SerializeObject(_validationOverrideRequests);

            _testContext.EmployerIncentivesApi.MockServer
               .Given(
                       Request
                       .Create()
                       .WithPath($"/api/validation-overrides")
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

            await _testContext.LegalEntitiesFunctions.HttpTriggerHandleValidationOverride.RunHttp(request);
        }
    

        [Then(@"the validation override is forwarded to the Employer Incentives API")]
        public void ThenTheEventIsForwardedToTheApi()
        {
            var requests = _testContext
                       .EmployerIncentivesApi
                       .MockServer
                       .FindLogEntries(
                           Request
                           .Create()
                           .WithPath($"/api/validation-overrides")
                           .WithBody(JsonConvert.SerializeObject(new ValidationOverrideRequest() { ValidationOverrides = _validationOverrideRequests.ToArray() }))
                           .UsingPost());

            requests.AsEnumerable().Count().Should().Be(1);
        }
    }    
}
