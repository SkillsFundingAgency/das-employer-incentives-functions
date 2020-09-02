using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Account
{
    public class WhenUpdateVrfCaseDetailsForNewApplications
    {
        private UpdateVrfCaseDetailsForNewApplications _sut;
        private Mock<ILegalEntitiesService> _mockILegalEntitiesService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockILegalEntitiesService = new Mock<ILegalEntitiesService>();
            _fixture = new Fixture();
            
            _sut = new UpdateVrfCaseDetailsForNewApplications(_mockILegalEntitiesService.Object);

        }

        [Test]
        public async Task Then_an_UpdateVrfCaseDetailsForNewApplications_Request_is_sent_to_the_LegalEntitiesService()
        {
            // Act
            await _sut.Run(null, null);

            // Assert
            _mockILegalEntitiesService.Verify(m => m.UpdateVrfCaseDetails(), Times.Once);
        }
    }
}