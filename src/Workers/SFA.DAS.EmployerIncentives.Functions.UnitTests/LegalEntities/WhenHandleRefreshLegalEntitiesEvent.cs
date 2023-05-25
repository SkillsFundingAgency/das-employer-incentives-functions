using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Messages.Events;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    public class WhenHandleRefreshLegalEntitiesEvent
    {
        private HandleRefreshLegalEntitiesEvent _sut;
        private Mock<ILegalEntitiesService> _mockLegalEntitiesService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockLegalEntitiesService = new Mock<ILegalEntitiesService>();
            _fixture = new Fixture();

            _mockLegalEntitiesService
                .Setup(m => m.Refresh(It.IsAny<int>(), It.IsAny<int>()))
                .Verifiable();

            _sut = new HandleRefreshLegalEntitiesEvent(_mockLegalEntitiesService.Object);

        }

        [Test]
        public async Task Then_a_RefreshLegalEntities_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var request = _fixture.Create<RefreshLegalEntitiesEvent>();

            // Act
            await _sut.RunEvent(request);

            // Assert
            _mockLegalEntitiesService
                .Verify(m => m.Refresh(request.PageNumber, request.PageSize)
                , Times.Once);
        }
    }
}