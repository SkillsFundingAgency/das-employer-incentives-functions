using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.Jobs
{
    public class WhenRefreshLegalEntities
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
            var pageNumber = _fixture.Create<int>();
            var pageSize = _fixture.Create<int>();
            var expected = new JobRequest
            {
                Type = JobType.RefreshLegalEntities,
                Data = new Dictionary<string, object>
                {
                    { "PageNumber", pageNumber },
                    { "PageSize", pageSize }
                }
            };

            // Act
            await _sut.RefreshLegalEntities(pageNumber, pageSize);

            // Assert
            _testClient.VerifyPutAsAsync("jobs", expected, Times.Once());
        }
    }
}