using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services
{
    public class WhenRefreshLegalEntities
    {
        private EmployerIncentivesService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;

        [SetUp]
        public void Setup()
        {          
            _baseAddress = new Uri(@"http://localhost");
            _testClient = new TestHttpClient(_baseAddress);

            _testClient.SetUpPutAsAsync(HttpStatusCode.OK);

            _sut = new EmployerIncentivesService(_testClient);
        }

        [Test]
        public async Task Then_the_default_job_request_is_forwarded_to_the_client()
        {
            // Arrange
            var expected = new JobRequest
            {
                Type = JobType.RefreshLegalEntities,
                Data = new Dictionary<string, object>
                {
                    { "PageNumber", 1 },
                    { "PageSize", 200 }
                }
            };

            // Act
            await _sut.RefreshLegalEntities();

            // Assert
            _testClient.VerifyPutAsAsync("jobs", expected, Times.Once());
        }
    }
}