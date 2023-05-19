using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using SFA.DAS.EmployerIncentives.Messages.Events;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    public class WhenHandleRefreshLegalEntityEvent
    {
        private HandleRefreshLegalEntityEvent _sut;
        private Mock<ILegalEntitiesService> _mockLegalEntitiesService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockLegalEntitiesService = new Mock<ILegalEntitiesService>();
            _fixture = new Fixture();

            _mockLegalEntitiesService
               .Setup(m => m.Add(It.IsAny<AddRequest>()))
               .Verifiable();

            _sut = new HandleRefreshLegalEntityEvent(_mockLegalEntitiesService.Object);

        }

        [Test]
        public async Task Then_an_AddLegalEntityRequest_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var request = _fixture.Create<RefreshLegalEntityEvent>();

            // Act
            await _sut.RunEvent(request);

            // Assert
            _mockLegalEntitiesService
               .Verify(m => m.Add(It.Is<AddRequest>(r =>
                   r.AccountId == request.AccountId &&
                   r.AccountLegalEntityId == request.AccountLegalEntityId &&
                   r.LegalEntityId == request.LegalEntityId &&
                   r.OrganisationName == request.OrganisationName))
               , Times.Once);
        }
    }
}