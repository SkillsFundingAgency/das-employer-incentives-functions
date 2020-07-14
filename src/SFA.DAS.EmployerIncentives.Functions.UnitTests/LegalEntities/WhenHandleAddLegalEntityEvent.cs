using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives.Types;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Account
{
    public class WhenHandleAddLegalEntityEvent
    {
        private HandleAddLegalEntityEvent _sut;
        private Mock<IEmployerIncentivesService> _mockEmployerIncentivesService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockEmployerIncentivesService = new Mock<IEmployerIncentivesService>();
            _fixture = new Fixture();

            _mockEmployerIncentivesService
                .Setup(m => m.AddLegalEntity(It.IsAny<AddLegalEntityRequest>()))
                .Verifiable();

            _sut = new HandleAddLegalEntityEvent(_mockEmployerIncentivesService.Object);

        }

        [Test]
        public async Task Then_an_AddLegalEntity_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var request = _fixture.Create<AddedLegalEntityEvent>();

            // Act
            await _sut.RunEvent(request);

            // Assert
            _mockEmployerIncentivesService
                .Verify(m => m.AddLegalEntity(It.Is<AddLegalEntityRequest>(r => 
                    r.AccountId == request.AccountId && 
                    r.AccountLegalEntityId == request.AccountLegalEntityId &&
                    r.LegalEntityId == request.LegalEntityId &&
                    r.OrganisationName == request.OrganisationName))
                ,Times.Once);                
        }
    }
}