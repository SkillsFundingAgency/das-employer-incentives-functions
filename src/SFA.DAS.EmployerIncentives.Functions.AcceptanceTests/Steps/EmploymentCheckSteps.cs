using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using NServiceBus.Transport;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Extensions;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs.Types;
using SFA.DAS.EmployerIncentives.Messages.Events;
using SFA.DAS.EmployerIncentives.Types;
using System;
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
    [Scope(Feature = "EmploymentCheck")]
    public class EmploymentCheckSteps : StepsBase    
    {
        private readonly TestContext _testContext;
        private readonly Fixture _fixture;
        private WaitForResult _messagePublishResult;
        private readonly EmploymentCheckRequest _employmentCheckRequest;
        private JobRequest _jobRequest;
        private const string InvalidErrorType = "Invalid";

        public EmploymentCheckSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _fixture = new Fixture();

            _employmentCheckRequest = new EmploymentCheckRequest
            {
                AccountLegalEntityId = _fixture.Create<long>(),
                ULN = _fixture.Create<long>(),
                ServiceRequest = _fixture.Create<ServiceRequest>()
            };
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

        [When(@"an employment check refresh request is received")]
        public async Task WhenAnEmploymentCheckRefreshRequestIsReceived()
        {
            _jobRequest = new JobRequest
            {
                Type = JobType.RefreshEmploymentCheck,
                Data = new System.Collections.Generic.Dictionary<string, string>
                    {
                        { "AccountLegalEntityId", _employmentCheckRequest.AccountLegalEntityId.ToString() },
                        { "ULN", _employmentCheckRequest.ULN.ToString() },
                        { "ServiceRequest", JsonConvert.SerializeObject(_employmentCheckRequest.ServiceRequest) }
                    }
            };

            _testContext.EmployerIncentivesApi.MockServer
               .Given(
                       Request
                       .Create()
                       .WithPath("/api/jobs")
                       .WithBody(JsonConvert.SerializeObject(_jobRequest))
                       .UsingPost()
                       )
                   .RespondWith(
               Response.Create()
                   .WithStatusCode(HttpStatusCode.OK)
                   .WithHeader("Content-Type", "application/json"));

            var request = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = new StringContent(JsonConvert.SerializeObject(_employmentCheckRequest), Encoding.UTF8, "application/json")
            };
            await _testContext.LegalEntitiesFunctions.HttpTriggerHandleRefreshEmploymentCheck.RunHttp(request);
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

        [Then(@"the request is forwarded to the Employer Incentives system")]
        public void ThenTheRequestIsForwardedToTheApi()
        {
            var requests = _testContext
                       .EmployerIncentivesApi
                       .MockServer
                       .FindLogEntries(
                           Request
                           .Create()
                           .WithPath("/api/jobs")
                           .WithBody(JsonConvert.SerializeObject(_jobRequest))
                           .UsingPut());

            requests.AsEnumerable().Count().Should().Be(1);
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
