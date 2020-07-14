using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Account
{
    public class WhenHandleAddLegalEntityEvent
    {
        private HandleAddLegalEntityEvent _sut;
        private Mock<ILegalEntitiesService> _mockILegalEntitiesService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockILegalEntitiesService = new Mock<ILegalEntitiesService>();
            _fixture = new Fixture();

            _mockILegalEntitiesService
                .Setup(m => m.Add(It.IsAny<AddRequest>()))
                .Verifiable();

            _sut = new HandleAddLegalEntityEvent(_mockILegalEntitiesService.Object);

        }

        [Test]
        public async Task Then_an_AddLegalEntity_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var request = _fixture.Create<AddedLegalEntityEvent>();

            // Act
            await _sut.RunEvent(request);

            // Assert
            _mockILegalEntitiesService
                .Verify(m => m.Add(It.Is<AddRequest>(r => 
                    r.AccountId == request.AccountId && 
                    r.AccountLegalEntityId == request.AccountLegalEntityId &&
                    r.LegalEntityId == request.LegalEntityId &&
                    r.OrganisationName == request.OrganisationName))
                ,Times.Once);                
        }
    }
}