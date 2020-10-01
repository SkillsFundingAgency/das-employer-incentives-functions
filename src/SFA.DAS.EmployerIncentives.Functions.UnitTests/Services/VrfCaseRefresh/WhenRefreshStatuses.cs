using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.VrfCaseRefresh
{
    public class WhenRefreshStatuses
    {
        private IVrfCaseRefreshService _sut;
        private readonly Mock<IVendorRegistrationFormService> _vrfService = new Mock<IVendorRegistrationFormService>();
        private readonly Mock<IVrfCaseRefreshRepository> _repository = new Mock<IVrfCaseRefreshRepository>();
        private readonly Mock<IDateTimeProvider> _dateTimeProvider = new Mock<IDateTimeProvider>();

        [SetUp]
        public void Setup()
        {
            _sut = new VrfCaseRefreshService(_vrfService.Object,
                _repository.Object,
                _dateTimeProvider.Object);
        }

        [Test]
        public async Task
            Then_API_is_invoked_with_LastRunDateTime_as_FromDateTime_and_current_DateTime_as_ToDateTime_and_last_Run_DateTime_is_updated()
        {
            // Arrange
            var lastRunDateTime = DateTime.UtcNow.AddHours(-1);
            _repository.Setup(x => x.GetLastRunDateTime()).ReturnsAsync(lastRunDateTime);
            var currentDateTime = DateTime.UtcNow;
            _dateTimeProvider.Setup(x => x.GetCurrentDateTime()).ReturnsAsync(currentDateTime);

            // Act
            await _sut.RefreshStatuses();

            // Assert
            _vrfService.Verify(x => x.Update(lastRunDateTime, currentDateTime), Times.Once());
            _repository.Verify(x => x.UpdateLastRunDateTime(currentDateTime), Times.Once);
        }
    }
}