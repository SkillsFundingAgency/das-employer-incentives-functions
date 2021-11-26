using FluentAssertions;
using Newtonsoft.Json;
using NServiceBus.Transport;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Extensions;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using SFA.DAS.EmployerIncentives.Messages.Events;
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
    [Scope(Feature = "EmploymentCheck")]
    public class EmploymentCheckSteps : StepsBase    
    {
        private readonly TestContext _testContext;
        private WaitForResult _messagePublishResult;

        public EmploymentCheckSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
        }

        [When(@"an employment check result is received with result (.*)")]
        public async Task WhenAnEmploymentCheckIsReceived(string result)
        {
            var completedEvent = _testContext.TestData.GetOrCreate(onCreate: () =>
            {
                return new EmploymentCheckCompletedEvent
                {
                    CorrelationId = Guid.NewGuid(),
                    Result = result.ToString(),
                    DateChecked = DateTime.UtcNow
                };
            });

            var updateRequest = _testContext.TestData.GetOrCreate(onCreate: () =>
                {
                    return new UpdateRequest
                    {
                        CorrelationId = completedEvent.CorrelationId,
                        Result = Map(result),
                        DateChecked = completedEvent.DateChecked
                    };
            });

            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                        Request
                        .Create()
                        .WithPath($"/api/employmentchecks/{updateRequest.CorrelationId}")
                        .WithBody(JsonConvert.SerializeObject(updateRequest))
                        .UsingPut()
                        )
                    .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("location", $"/api/employmentchecks/{updateRequest.CorrelationId}"));
            
           _messagePublishResult = await _testContext.WaitFor<MessageContext>(async () => 
                await _testContext.TestMessageBus.Publish(completedEvent));
        }

        [Then(@"the event is forwarded to the Employer Incentives system")]
        public void ThenTheEventIsForwardedToTheApi()
        {
            var updateRequest = _testContext.TestData.GetOrCreate<UpdateRequest>();

            var requests = _testContext
                       .EmployerIncentivesApi
                       .MockServer
                       .FindLogEntries(
                           Request
                           .Create()
                           .WithPath($"/api/employmentchecks/{updateRequest.CorrelationId}")
                           .WithBody(JsonConvert.SerializeObject(updateRequest))
                           .UsingPut());

            requests.AsEnumerable().Count().Should().Be(1);
        }

        [Then(@"the event is not forwarded to the Employer Incentives system")]
        public void ThenTheEventIsNotForwardedToTheApi()
        {
            var updateRequest = _testContext.TestData.GetOrCreate<UpdateRequest>();

            var requests = _testContext
                       .EmployerIncentivesApi
                       .MockServer
                       .FindLogEntries(
                           Request
                           .Create()
                           .WithPath($"/api/employmentchecks/{updateRequest.CorrelationId}")
                           .WithBody(JsonConvert.SerializeObject(updateRequest))
                           .UsingPut());

            requests.AsEnumerable().Count().Should().Be(0);

            _messagePublishResult.HasErrored.Should().BeTrue();
        }

        private EmploymentCheckResult Map(string result)
        {
            return result.ToLower() switch
            {
                "employed" => EmploymentCheckResult.Employed,
                "not employed" => EmploymentCheckResult.NotEmployed,
                "hmrc unknown" => EmploymentCheckResult.HMRCUnknown,
                "no nino found" => EmploymentCheckResult.NoNINOFound,
                _ => EmploymentCheckResult.NoAccountFound,
            };
        }
    }    
}
