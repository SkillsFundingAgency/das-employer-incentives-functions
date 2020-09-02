using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs.Types;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.Jobs
{
    public class WhenUpdateVrfCaseDetails
    {
        private JobsService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _baseAddress = new Uri(@"http://localhost");
            _testClient = new TestHttpClient(_baseAddress);

            _testClient.SetUpPutAsAsync(HttpStatusCode.OK);

            _sut = new JobsService(_testClient);
        }

        [Test]
        public async Task Then_the_default_job_request_is_forwarded_to_the_client()
        {
            // Arrange
            var expected = new JobRequest
            {
                Type = JobType.UpdateVrfCaseDetailsForNewApplications,
                Data = null
            };

            // Act
            await _sut.UpdateVrfCaseDetailsForNewApplications();

            // Assert
            _testClient.VerifyPutAsAsync("jobs", expected, Times.Once());
        }
    }
}