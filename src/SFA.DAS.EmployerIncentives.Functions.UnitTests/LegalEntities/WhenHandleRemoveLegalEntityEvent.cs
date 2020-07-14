using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives.Types;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    public class WhenHandleRemoveLegalEntityEvent
    {
        private HandleRemoveLegalEntityEvent _sut;
        private Mock<IEmployerIncentivesService> _mockEmployerIncentivesService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockEmployerIncentivesService = new Mock<IEmployerIncentivesService>();
            _fixture = new Fixture();

            _mockEmployerIncentivesService
                .Setup(m => m.RemoveLegalEntity(It.IsAny<RemoveLegalEntityRequest>()))
                .Verifiable();

            _sut = new HandleRemoveLegalEntityEvent(_mockEmployerIncentivesService.Object);
        }

        [Test]
        public async Task Then_a_RefreshLegalEntities_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var request = _fixture.Create<RemovedLegalEntityEvent>();

            // Act
            await _sut.RunEvent(request);

            // Assert
            _mockEmployerIncentivesService
                .Verify(m => m.RemoveLegalEntity(It.Is<RemoveLegalEntityRequest>(r => 
                    r.AccountId == request.AccountId &&
                    r.AccountLegalEntityId == request.AccountLegalEntityId
                    ))
                , Times.Once);
        }
    }
}