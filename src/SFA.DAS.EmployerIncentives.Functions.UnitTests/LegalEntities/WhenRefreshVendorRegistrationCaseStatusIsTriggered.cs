using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    public class WhenRefreshVendorRegistrationCaseStatusIsTriggered
    {
        private RefreshVendorRegistrationCaseStatus _sut;
        private readonly Mock<IVendorRegistrationFormService> _mockIVendorRegistrationFormService = new Mock<IVendorRegistrationFormService>();

        [SetUp]
        public void Setup()
        {
            _sut = new RefreshVendorRegistrationCaseStatus(_mockIVendorRegistrationFormService.Object);
        }

        [Test]
        public async Task Then_service_is_invoked()
        {
            // Act
            await _sut.Run(null);

            // Assert
            _mockIVendorRegistrationFormService.Verify(m => m.RefreshVendorRegistrationFormStatuses(), Times.Once);
        }

    }
}