using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FluentAssertions;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.VrfCaseRefresh
{
    public class WhenResume
    {
        private IVrfCaseRefreshService _sut;
        private readonly Mock<IVendorRegistrationFormService> _vrfService = new Mock<IVendorRegistrationFormService>();
        private readonly Mock<IVrfCaseRefreshRepository> _repository = new Mock<IVrfCaseRefreshRepository>();

        [SetUp]
        public void Setup()
        {
            _sut = new VrfCaseRefreshService(_vrfService.Object, _repository.Object, Mock.Of<ILogger<VrfCaseRefreshService>>());
        }

        [Test]
        public async Task Then_the_VRF_refresh_service_is_resumed_when_paused()
        {
            // Arrange
            var lastRunDateTime = DateTime.UtcNow.AddHours(-1);
            var vrfCaseRefresh = new Functions.LegalEntities.Services.LegalEntities.Types.VrfCaseRefresh 
            { 
                LastRunDateTime = lastRunDateTime,
                IsPaused = true
            };
            
            _repository.Setup(x => x.Get()).ReturnsAsync(vrfCaseRefresh);

            // Act
            await _sut.Resume();

            // Assert
            _repository.Verify(x => x.Update(vrfCaseRefresh), Times.Once);
            vrfCaseRefresh.IsPaused.Should().BeFalse();
        }

        [Test]
        public async Task Then_the_VRF_refresh_service_is_resumed_when_already_resumed()
        {
            // Arrange
            var lastRunDateTime = DateTime.UtcNow.AddHours(-1);
            var vrfCaseRefresh = new Functions.LegalEntities.Services.LegalEntities.Types.VrfCaseRefresh
            {
                LastRunDateTime = lastRunDateTime,
                IsPaused = false
            };

            _repository.Setup(x => x.Get()).ReturnsAsync(vrfCaseRefresh);

            // Act
            await _sut.Resume();

            // Assert
            _repository.Verify(x => x.Update(vrfCaseRefresh), Times.Once);
            vrfCaseRefresh.IsPaused.Should().BeFalse();
        }
    }
}