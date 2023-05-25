using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Types;
using SFA.DAS.EmployerIncentives.Functions.Support.AcceptanceTests.Services;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.EmploymentCheck.Types;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs.Types;
using SFA.DAS.EmployerIncentives.Types;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.Support.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "EmploymentCheck")]
    public class EmploymentCheckSteps : StepsBase    
    {
        private readonly TestContext _testContext;
        private readonly Fixture _fixture;
        private readonly List<EmploymentCheckRequest> _employmentCheckRequests;
        private JobRequest _jobRequest;

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
        
        [When(@"an (.*) employment check refresh request is received")]
        public async Task WhenAnEmploymentCheckRefreshRequestIsReceived(string checkType)
        {
            _employmentCheckRequests[0].CheckType = checkType;
            _jobRequest = new JobRequest
            {
                Type = JobType.RefreshEmploymentChecks,
                Data = new Dictionary<string, string>
                    {
                        { "Requests", JsonConvert.SerializeObject(_employmentCheckRequests) }
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
                Content = new StringContent(JsonConvert.SerializeObject(_employmentCheckRequests), Encoding.UTF8, "application/json")
            };
            await _testContext.LegalEntitiesFunctions.HttpTriggerHandleRefreshEmploymentCheck.RunHttp(request);
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

    }    
}