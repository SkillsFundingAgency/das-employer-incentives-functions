using FluentAssertions;
using Newtonsoft.Json;
using NServiceBus.Transport;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Extensions;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs.Types;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using SFA.DAS.EmployerIncentives.Messages.Events;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "LegalEntities")]
    public class LegalEntitySteps : StepsBase    
    {
        private readonly TestContext _testContext;
        private string requestType;

        public LegalEntitySteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
        }

        [When(@"a legal entity is added to an account")]
        public async Task WhenAddedLegalEntityEventIsReceived()
        {
            requestType = "added";
            var addedEvent = _testContext.TestData.GetOrCreate<AddedLegalEntityEvent>();
            var addLegalEntityRequest = _testContext.TestData.GetOrCreate(onCreate: () =>
                {
                    return new AddRequest
                    {
                        AccountId = addedEvent.AccountId,
                        AccountLegalEntityId = addedEvent.AccountLegalEntityId,
                        LegalEntityId = addedEvent.LegalEntityId,
                        OrganisationName = addedEvent.OrganisationName
                    };
            });

            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                        Request
                        .Create()
                        .WithPath($"/api/accounts/{addLegalEntityRequest.AccountId}/legalEntities")
                        .WithBody(JsonConvert.SerializeObject(addLegalEntityRequest))
                        .UsingPost()
                        )
                    .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("location", $"/api/accounts/{addLegalEntityRequest.AccountId}/LegalEntities"));
            
           await _testContext.WaitFor<MessageContext>(async () => 
                await _testContext.TestMessageBus.Publish(addedEvent));
        }

        [When(@"a legal entity is removed from an account")]
        public async Task WhenRemovedLegalEntityEventIsReceived()
        {
            requestType = "removed";
            var removedEvent = _testContext.TestData.GetOrCreate<RemovedLegalEntityEvent>();

            _testContext.EmployerIncentivesApi.MockServer
                 .Given(
                         Request
                         .Create()
                         .WithPath($"/api/accounts/{removedEvent.AccountId}/legalEntities/{removedEvent.AccountLegalEntityId}")
                         .UsingDelete()
                         )
                     .RespondWith(
                 Response.Create()
                     .WithStatusCode(HttpStatusCode.OK)
                     .WithHeader("Content-Type", "application/json"));

            await _testContext.WaitFor<MessageContext>(async () =>
                 await _testContext.TestMessageBus.Publish(removedEvent));
        }

        [When(@"a request to refresh legal entities is received")]
        public async Task WhenRefreshLegalEntitiesRequestIsReceived()
        {
            var jobRequest = _testContext.TestData.GetOrCreate(onCreate: () =>
            {
                return new JobRequest
                {
                    Type = JobType.RefreshLegalEntities,
                    Data = new Dictionary<string, object>
                    {
                        { "PageNumber", 1 },
                        { "PageSize", 200 }
                    }
                };
            });

            _testContext.EmployerIncentivesApi.MockServer
               .Given(
                       Request
                       .Create()
                       .WithPath($"/api/jobs")
                       .WithBody(JsonConvert.SerializeObject(jobRequest))
                       .UsingPut()
                       )
                   .RespondWith(
               Response.Create()
                   .WithStatusCode(HttpStatusCode.OK)
                   .WithHeader("Content-Type", "application/json"));

            await _testContext.LegalEntitiesFunctions.HttpTriggerRefreshLegalEntities.RunHttp(null);
        }

        [When(@"a request to refresh a page of legal entities is received")]
        public async Task WhenRefreshLegalEntitiesEventIsReceived()
        {
            requestType = "refresh";
            var refreshEvent = _testContext.TestData.GetOrCreate<RefreshLegalEntitiesEvent>();

            var jobRequest = _testContext.TestData.GetOrCreate(onCreate: () =>
            {
                return new JobRequest
                {
                    Type = JobType.RefreshLegalEntities,
                    Data = new Dictionary<string, object>
                    {
                        { "PageNumber", refreshEvent.PageNumber },
                        { "PageSize", refreshEvent.PageSize }
                    }
                };
            });

            _testContext.EmployerIncentivesApi.MockServer
                 .Given(
                         Request
                         .Create()
                         .WithPath($"/api/jobs")
                         .WithBody(JsonConvert.SerializeObject(jobRequest))
                         .UsingPut()
                         )
                     .RespondWith(
                 Response.Create()
                     .WithStatusCode(HttpStatusCode.OK)
                     .WithHeader("Content-Type", "application/json"));

            await _testContext.WaitFor<MessageContext>(async () =>
                 await _testContext.TestMessageBus.Publish(refreshEvent));
        }

        [When(@"a request to refresh a legal entity is received")]
        public async Task WhenRefreshLegalEntityEventIsReceived()
        {
            requestType = "refreshLegalEntity";
            var refreshEvent = _testContext.TestData.GetOrCreate<RefreshLegalEntityEvent>();

            var addLegalEntityRequest = _testContext.TestData.GetOrCreate(onCreate: () =>
            {
                return new AddRequest
                {
                    AccountId = refreshEvent.AccountId,
                    AccountLegalEntityId = refreshEvent.AccountLegalEntityId,
                    LegalEntityId = refreshEvent.LegalEntityId,
                    OrganisationName = refreshEvent.OrganisationName
                };
            });

            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                        Request
                        .Create()
                        .WithPath($"/api/accounts/{addLegalEntityRequest.AccountId}/legalEntities")
                        .WithBody(JsonConvert.SerializeObject(addLegalEntityRequest))
                        .UsingPost()
                        )
                    .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("location", $"/api/accounts/{addLegalEntityRequest.AccountId}/LegalEntities"));

            await _testContext.WaitFor<MessageContext>(async () =>
                 await _testContext.TestMessageBus.Publish(refreshEvent));
        }

        [Then(@"the event is forwarded to the Employer Incentives system")]
        public void ThenTheEventIsForwardedToTheApi()
        {
            switch(requestType)
            {
                case "added":
                case "refreshLegalEntity":                    
                    ThenTheAddedEventIsForwardedToTheApi();
                    break;
                case "removed":
                    ThenTheRemovedEventIsForwardedToTheApi();
                    break;
                case "refresh":
                    ThenTheRefreshRequestIsForwardedToTheApi();
                    break;
            }
        }

        public void ThenTheAddedEventIsForwardedToTheApi()
        {
            var addLegalEntityRequest = _testContext.TestData.GetOrCreate<AddRequest>();

            var requests = _testContext
                       .EmployerIncentivesApi
                       .MockServer
                       .FindLogEntries(
                           Request
                           .Create()
                           .WithPath($"/api/accounts/{addLegalEntityRequest.AccountId}/legalEntities")
                           .WithBody(JsonConvert.SerializeObject(addLegalEntityRequest))
                           .UsingPost());
            
            requests.AsEnumerable().Count().Should().Be(1);
        }

        public void ThenTheRemovedEventIsForwardedToTheApi()
        {
            var removedEvent = _testContext.TestData.GetOrCreate<RemovedLegalEntityEvent>();

            var requests = _testContext
                       .EmployerIncentivesApi
                       .MockServer
                       .FindLogEntries(
                           Request
                           .Create()
                           .WithPath($"/api/accounts/{removedEvent.AccountId}/legalEntities/{removedEvent.AccountLegalEntityId}")
                         .UsingDelete());

            requests.AsEnumerable().Count().Should().Be(1);
        }

        [Then(@"the request is forwarded to the Employer Incentives system")]
        public void ThenTheRefreshRequestIsForwardedToTheApi()
        {
            var jobRequest = _testContext.TestData.GetOrCreate<JobRequest>();

            var requests = _testContext
                       .EmployerIncentivesApi
                       .MockServer
                       .FindLogEntries(
                           Request
                           .Create()
                           .WithPath($"/api/jobs")
                            .WithBody(JsonConvert.SerializeObject(jobRequest))
                            );

            requests.AsEnumerable().Count().Should().Be(1);
        }
    }    
}
