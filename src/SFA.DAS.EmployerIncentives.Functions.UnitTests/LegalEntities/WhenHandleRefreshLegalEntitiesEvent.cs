using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives;
using SFA.DAS.EmployerIncentives.Messages.Events;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    public class WhenHandleRefreshLegalEntitiesEvent
    {
        private HandleRefreshLegalEntitiesEvent _sut;
        private Mock<IEmployerIncentivesService> _mockEmployerIncentivesService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockEmployerIncentivesService = new Mock<IEmployerIncentivesService>();
            _fixture = new Fixture();

            _mockEmployerIncentivesService
                .Setup(m => m.RefreshLegalEntities(It.IsAny<int>(), It.IsAny<int>()))
                .Verifiable();

            _sut = new HandleRefreshLegalEntitiesEvent(_mockEmployerIncentivesService.Object);

        }

        [Test]
        public async Task Then_a_RefreshLegalEntities_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var request = _fixture.Create<RefreshLegalEntitiesEvent>();

            // Act
            await _sut.RunEvent(request);

            // Assert
            _mockEmployerIncentivesService
                .Verify(m => m.RefreshLegalEntities(request.PageNumber, request.PageSize)
                , Times.Once);
        }
    }
}