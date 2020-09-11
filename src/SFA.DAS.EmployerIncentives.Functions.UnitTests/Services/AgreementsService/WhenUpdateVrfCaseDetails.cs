using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.HashingService;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.AgreementsService
{
    public class WhenUpdateVrfCaseDetails
    {
        private VendorRegistrationFormService _sut;
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

            _sut = new VendorRegistrationFormService(_testClient, _mockJobsService.Object, Mock.Of<IHashingService>());
        }

        [Test]
        public async Task Then_the_default_job_request_is_forwarded_to_the_jobs_service()
        {
            // Act
            await _sut.UpdateVrfCaseDetails();

            // Assert
            _mockJobsService.Verify(m => m.UpdateVrfCaseDetailsForNewApplications());
        }
    }
}