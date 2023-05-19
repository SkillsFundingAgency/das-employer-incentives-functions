using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using NServiceBus.Transport;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Extensions;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs.Types;
using SFA.DAS.EmployerIncentives.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Types;
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
        private readonly Fixture _fixture;
        private WaitForResult _messagePublishResult;
        private readonly List<EmploymentCheckRequest> _employmentCheckRequests;
        private JobRequest _jobRequest;
        private const string InvalidErrorType = "Invalid";

        public EmploymentCheckSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _fixture = new Fixture();

            var employmentCheckRequest = new EmploymentCheckRequest
            {
                CheckType = RefreshEmploymentCheckType.InitialEmploymentChecks.ToString(),
                Applications = new List<Application>
                {
                    new Application {AccountLegalEntityId = _fixture.Create<long>(), ULN = _fixture.Create<long>()}
                }.ToArray(),
                ServiceRequest = _fixture.Create<ServiceRequest>()
            };
            _employmentCheckRequests = new List<EmploymentCheckRequest> { employmentCheckRequest };
        }

        [When(@"an employment check result is received with an invalid error type")]
        public Task WhenAnEmploymentCheckIsReceivedWithAnInvalidErrorType()
        {
            return WhenAnEmploymentCheckIsReceived(null, InvalidErrorType);
        }

        [When(@"an employment check result is received with (.*) and (.*)")]
        public async Task WhenAnEmploymentCheckIsReceived(bool? employmentResult, string errorType)
        {
            var completedEvent = _testContext.TestData.GetOrCreate(onCreate: () =>
            {
                return new EmploymentCheck.Types.EmploymentCheckCompletedEvent(
                    Guid.NewGuid(),
                    employmentResult,
                    DateTime.UtcNow,
                    errorType == "null"? null : errorType);
            });

            var updateRequest = _testContext.TestData.GetOrCreate(onCreate: () =>
                {
                    var request =  new UpdateRequest
                    {
                        CorrelationId = completedEvent.CorrelationId,
                        DateChecked = completedEvent.CheckDate
                    };
                    if (errorType  != InvalidErrorType)
                    {
                        request.Result = Map(employmentResult, errorType).ToString();
                    }
                    return request;
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

        private EmploymentCheckResult Map(bool? result, string errorType)
        {
            if (result.HasValue)
            {
                return result.Value ? EmploymentCheckResult.Employed : EmploymentCheckResult.NotEmployed;
            }

            return errorType.ToLower() switch
            {
                "ninonotfound" => EmploymentCheckResult.NinoNotFound,
                "ninofailure" => EmploymentCheckResult.NinoFailure,
                "ninoinvalid" => EmploymentCheckResult.NinoInvalid,
                "payenotfound" => EmploymentCheckResult.PAYENotFound,
                "payefailure" => EmploymentCheckResult.PAYEFailure,
                "ninoandpayenotfound" => EmploymentCheckResult.NinoAndPAYENotFound,
                "hmrcfailure" => EmploymentCheckResult.HmrcFailure,
                _ => throw new ArgumentException($"Unsupported Employment Check result : {result}"),
            };
        }
    }    
}
