using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    public class WhenRefreshVendorRegistrationCaseStatusIsTriggered
    {
        private RefreshVendorRegistrationCaseStatus _sut;
        private readonly Mock<IVendorRegistrationFormService> _mockIVendorRegistrationFormService = new Mock<IVendorRegistrationFormService>();
        private readonly Mock<IVrfCaseRefreshConfiguration> _mockVrfRefreshConfiguration = new Mock<IVrfCaseRefreshConfiguration>();
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new Mock<IDateTimeProvider>();
        private readonly Mock<ILogger> _mockLogger = new Mock<ILogger>();

        [SetUp]
        public void Setup()
        {
            _sut = new RefreshVendorRegistrationCaseStatus(_mockIVendorRegistrationFormService.Object, _mockVrfRefreshConfiguration.Object,
                _mockDateTimeProvider.Object);
        }

        [Test]
        public async Task Then_service_is_invoked_with_LastRunDateTime_as_FromDateTime_and_current_DateTime_as_ToDateTime()
        {
            // Arrange
            var lastRunDateTime = DateTime.UtcNow.AddHours(-1);
            _mockVrfRefreshConfiguration.Setup(x => x.GetLastRunDateTime()).ReturnsAsync(lastRunDateTime);
            var currentDateTime = DateTime.UtcNow;
            _mockDateTimeProvider.Setup(x => x.GetCurrentDateTime()).ReturnsAsync(currentDateTime);

            // Act
            await _sut.Run(null, _mockLogger.Object);

            // Assert
            _mockIVendorRegistrationFormService.Verify(m => m.RefreshVendorRegistrationFormStatuses(
                lastRunDateTime, currentDateTime), Times.Once);
            _mockVrfRefreshConfiguration.Verify(x => x.UpdateLastRunDateTime(currentDateTime), Times.Once);
        }

    }
}