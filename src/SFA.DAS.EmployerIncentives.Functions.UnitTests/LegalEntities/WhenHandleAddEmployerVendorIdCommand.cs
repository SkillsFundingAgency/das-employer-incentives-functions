using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Commands;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    public class WhenHandleAddEmployerVendorIdCommand
    {
        private HandleAddEmployerVendorIdCommand _sut;
        private Mock<IEmployerVendorIdService> _mockEmployerVendorIdService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockEmployerVendorIdService = new Mock<IEmployerVendorIdService>();
            _fixture = new Fixture();

            _mockEmployerVendorIdService
                .Setup(m => m.AddEmployerVendorId(It.IsAny<string>()))
                .Verifiable();

            _sut = new HandleAddEmployerVendorIdCommand(_mockEmployerVendorIdService.Object);
        }

        [Test]
        public async Task Then_an_AddEmployerVendorId_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var command = _fixture.Create<AddEmployerVendorIdCommand>();

            // Act
            await _sut.RunEvent(command);

            // Assert
            _mockEmployerVendorIdService.Verify(m => m.AddEmployerVendorId(command.HashedLegalEntityId), Times.Once);                
        }
    }
}