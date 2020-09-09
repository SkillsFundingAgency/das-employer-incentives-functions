using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Account
{
    public class WhenUpdateVrfCaseDetailsForNewApplications
    {
        private UpdateVrfCaseDetailsForNewApplications _sut;
        private Mock<IVendorRegistrationFormService> _mockIVendorRegistrationFormService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockIVendorRegistrationFormService = new Mock<IVendorRegistrationFormService>();
            _fixture = new Fixture();
            
            _sut = new UpdateVrfCaseDetailsForNewApplications(_mockIVendorRegistrationFormService.Object);

        }

        [Test]
        public async Task Then_an_UpdateVrfCaseDetailsForNewApplications_Request_is_sent_to_the_LegalEntitiesService()
        {
            // Act
            await _sut.Run(null, null);

            // Assert
            _mockIVendorRegistrationFormService.Verify(m => m.UpdateVrfCaseDetails(), Times.Once);
        }
    }
}