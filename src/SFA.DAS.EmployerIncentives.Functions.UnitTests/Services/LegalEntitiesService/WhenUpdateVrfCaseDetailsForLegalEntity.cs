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
    public class WhenUpdateVrfCaseDetailsForLegalEntity
    {
        private LegalEntitiesService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;
        private Mock<IJobsService> _mockJobsService;
        private Fixture _fixture;
        private Mock<IHashingService> _mockHashingService;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _baseAddress = new Uri(@"http://localhost");
            _testClient = new TestHttpClient(_baseAddress);

            _mockJobsService = new Mock<IJobsService>();
            _mockHashingService = new Mock<IHashingService>();

            _testClient.SetUpPatchAsAsync(HttpStatusCode.NoContent);

            _sut = new LegalEntitiesService(_testClient, _mockJobsService.Object, _mockHashingService.Object);
        }

        [Test]
        public async Task Then_the_default_job_request_is_forwarded_to_the_jobs_service()
        {
            // Arrange
            var legalEntityId = _fixture.Create<long>();
            var hashedLegalEntityId = _fixture.Create<string>();

            _mockHashingService.Setup(x => x.HashValue(legalEntityId)).Returns(hashedLegalEntityId);

            // Act
            await _sut.UpdateVrfCaseDetails(legalEntityId);

            // Assert
            _testClient.VerifyPatchAsAsync($"legalentities/{legalEntityId}/vendorregistrationform?hashedLegalEntityId={hashedLegalEntityId}", Times.Once());
        }
    }
}