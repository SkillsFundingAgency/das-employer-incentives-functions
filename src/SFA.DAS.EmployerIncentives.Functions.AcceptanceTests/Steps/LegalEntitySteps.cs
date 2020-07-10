using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives.Types;
using System;
using System.Linq;
using System.Net;
using System.Threading;
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
        private readonly AddedLegalEntityEvent _addedEvent;
        private readonly AddLegalEntityRequest _addLegalEntityRequest;
        private bool hasTimedOut = false;

        public LegalEntitySteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _addedEvent = _testContext.TestData.GetOrCreate<AddedLegalEntityEvent>();
            _addLegalEntityRequest = new AddLegalEntityRequest
            {
                AccountId = _addedEvent.AccountId,
                AccountLegalEntityId = _addedEvent.AccountLegalEntityId,
                LegalEntityId = _addedEvent.LegalEntityId,
                OrganisationName = _addedEvent.OrganisationName
            };
        }

        [When(@"a legal entity is added to an account")]
        public async Task WhenAddedLegalEntityEventIsReceived()
        {
            _testContext.EmployerIncentivesApi.MockServer
                .Given(
                        Request
                        .Create()
                        .WithPath($"/accounts/{_addLegalEntityRequest.AccountId}/legalEntities")
                        .WithBody(JsonConvert.SerializeObject(_addLegalEntityRequest))
                        .UsingPost()
                        )
                    .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("location", $"/accounts/{_addLegalEntityRequest.AccountId}/LegalEntities"));

            await _testContext.WaitForMessage(async () => await _testContext.TestMessageBus.Publish(_addedEvent));            
        }

        [When(@"a legal entity is removed from an account")]
        public Task WhenRemovedLegalEntityEventIsReceived()
        {
            // blank
            return Task.CompletedTask;
        }

        [When(@"a request to refresh legal entities is received")]
        public Task WhenRefreshLegalEntitiesRequestIsReceived()
        {
            // blank
            return Task.CompletedTask;
        }

        [When(@"a request to refresh a page of legal entities is received")]
        public Task WhenRefreshLegalEntitiesEventIsReceived()
        {
            // blank
            return Task.CompletedTask;
        }

        [Then(@"the event is forwarded to the Employer Incentives system")]
        public void ThenTheEventIsForwardedToTheApi()
        {              
            var requests = _testContext
                       .EmployerIncentivesApi
                       .MockServer
                       .FindLogEntries(
                           Request
                           .Create()
                           .WithPath($"/accounts/{_addLegalEntityRequest.AccountId}/legalEntities")
                           .WithBody(JsonConvert.SerializeObject(_addLegalEntityRequest))
                           .UsingPost());            
            requests.AsEnumerable().Count().Should().Be(1);
        }

        [Then(@"the request is forwarded to the Employer Incentives system")]
        public Task ThenTheRefreshRequestIsForwardedToTheApi()
        {
            // blank
            return Task.CompletedTask;
        }

        private void TimedOutCallback(object state)
        {
            hasTimedOut = true;
        }
    }
}
