using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    public class WhenHandleRefreshLegalEntitiesRequest
    {
        private HandleRefreshLegalEntitiesRequest _sut;
        private Mock<ILegalEntitiesService> _mockLegalEntitiesService;        

        [SetUp]
        public void Setup()
        {
            _mockLegalEntitiesService = new Mock<ILegalEntitiesService>();

            _mockLegalEntitiesService
                .Setup(m => m.Refresh())
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            _sut = new HandleRefreshLegalEntitiesRequest(_mockLegalEntitiesService.Object);
        }

        [Test]
        public async Task Then_a_RefreshLegalEntities_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var request = TestHttpClient.CreateHttpRequest("");

            // Act
            await _sut.RunHttp(request);

            // Assert
            _mockLegalEntitiesService
                .Verify(m => m.Refresh()
                , Times.Once);
        }

        [Test]
        public async Task Then_an_OkResult_is_returned_on_success()
        {
            // Arrange
            var request = TestHttpClient.CreateHttpRequest("");

            // Act
            var response = await _sut.RunHttp(request) as OkResult;

            // Assert
            response.Should().NotBeNull();
        }
    }
}