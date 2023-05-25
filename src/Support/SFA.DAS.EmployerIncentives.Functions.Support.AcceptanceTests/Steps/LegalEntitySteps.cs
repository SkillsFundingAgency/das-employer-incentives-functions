using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs.Types;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.Support.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "LegalEntities")]
    public class LegalEntitySteps : StepsBase    
    {
        private readonly TestContext _testContext;
        
        public LegalEntitySteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
        }

        [When(@"a request to refresh legal entities is received")]
        public async Task WhenRefreshLegalEntitiesRequestIsReceived()
        {
            var jobRequest = _testContext.TestData.GetOrCreate(onCreate: () =>
            {
                return new JobRequest
                {
                    Type = JobType.RefreshLegalEntities,
                    Data = new Dictionary<string, string>
                    {
                        { "PageNumber", "1" },
                        { "PageSize", "200" }
                    }
                };
            });

            _testContext.EmployerIncentivesApi.MockServer
               .Given(
                       Request
                       .Create()
                       .WithPath(x => x.Contains("legalentities/refresh"))
                       .UsingPut()
                       )
                   .RespondWith(
               Response.Create()
                   .WithStatusCode(HttpStatusCode.OK)
                   .WithHeader("Content-Type", "application/json"));

            await _testContext.LegalEntitiesFunctions.HttpTriggerRefreshLegalEntities.RunHttp(null);
        }

        [Then(@"a request is made to the Employer Incentives system")]
        public void TheARequestIsMadeToTheApi()
        {
            var jobRequest = _testContext.TestData.GetOrCreate<JobRequest>();

            var requests = _testContext
                .EmployerIncentivesApi
                .MockServer
                .FindLogEntries(
                    Request
                        .Create()
                        .WithPath($"/api/jobs")
                        .WithBody(JsonConvert.SerializeObject((object)jobRequest))
                );

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
                           .WithPath(x => x.Contains("legalentities/refresh")));

            requests.AsEnumerable().Count().Should().Be(1);
        }
    }    
}
