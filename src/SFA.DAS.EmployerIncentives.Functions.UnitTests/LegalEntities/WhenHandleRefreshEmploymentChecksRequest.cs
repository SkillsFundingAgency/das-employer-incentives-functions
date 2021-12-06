using System.Net.Http;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    public class WhenHandleRefreshEmploymentChecksRequest
    {
        private HandleRefreshEmploymentChecksRequest _sut;
        private Mock<IJobsService> _mockJobsService;        

        [SetUp]
        public void Setup()
        {
            _mockJobsService = new Mock<IJobsService>();

            _mockJobsService
                .Setup(m => m.RefreshEmploymentChecks())
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            _sut = new HandleRefreshEmploymentChecksRequest(_mockJobsService.Object);
        }

        [Test]
        public async Task Then_a_RefreshEmploymentChecks_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/HttpTriggerRefreshEmploymentChecks")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            // Act
            await _sut.RunHttp(request);

            // Assert
            _mockJobsService
                .Verify(m => m.RefreshEmploymentChecks()
                , Times.Once);
        }

        [Test]
        public async Task Then_an_OkResult_is_returned_on_success()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/HttpTriggerRefreshEmploymentChecks")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _sut.RunHttp(request) as OkResult;

            // Assert
            response.Should().NotBeNull();
        }
    }
}