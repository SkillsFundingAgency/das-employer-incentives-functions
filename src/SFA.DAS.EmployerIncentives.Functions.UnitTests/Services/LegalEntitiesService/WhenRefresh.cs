using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using SFA.DAS.HashingService;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.LegalEntities
{
    public class WhenRefresh
    {
        private LegalEntitiesService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;
        private Mock<IJobsService> _mockJobsService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _baseAddress = new Uri(@"http://localhost");
            _testClient = new TestHttpClient(_baseAddress);

            _mockJobsService = new Mock<IJobsService>();

            _testClient.SetUpPutAsAsync(HttpStatusCode.OK);

            _sut = new LegalEntitiesService(_testClient, _mockJobsService.Object);
        }

        [Test]
        public async Task Then_the_default_job_request_is_forwarded_to_the_jobs_service()
        {
            // Arrange
            var defaultPageNumber = 1;
            var defaultPageSize = 200;           

            // Act
            await _sut.Refresh();

            // Assert
            _mockJobsService.Verify(m => m.RefreshLegalEntities(defaultPageNumber, defaultPageSize));
        }

        [Test]
        public async Task Then_the_job_request_is_forwarded_to_the_jobs_service()
        {
            // Arrange
            var pageNumber = _fixture.Create<int>();
            var pageSize = _fixture.Create<int>();

            // Act
            await _sut.Refresh(pageNumber, pageSize);

            // Assert
            _mockJobsService.Verify(m => m.RefreshLegalEntities(pageNumber, pageSize));
        }
    }
}