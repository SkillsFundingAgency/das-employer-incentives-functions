using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RecalculateEarnings;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RecalculateEarnings.Types;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    [TestFixture]
    public class WhenHandleRecalculateEarningsRequest
    {
        private HandleRecalculateEarningsRequest _sut;
        private Mock<IRecalculateEarningsService> _mockService;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IRecalculateEarningsService>();

            _mockService
                .Setup(m => m.RecalculateEarnings(It.IsAny<RecalculateEarningsRequest>()))
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            _sut = new HandleRecalculateEarningsRequest(_mockService.Object);
        }

        [Test]
        public async Task Then_a_RecalculateEarnings_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/recalculate-earnings")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            // Act
            await _sut.RunHttp(request);

            // Assert
            _mockService
                .Verify(m => m.RecalculateEarnings(It.IsAny<RecalculateEarningsRequest>())
                    , Times.Once);
        }

        [Test]
        public async Task Then_an_OkResult_is_returned_on_success()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/recalculate-earnings")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _sut.RunHttp(request) as OkResult;

            // Assert
            response.Should().NotBeNull();
        }

        [Test]
        public async Task Then_an_error_response_is_returned_for_invalid_input()
        {
            // Arrange
            _mockService.Setup(x => x.RecalculateEarnings(It.IsAny<RecalculateEarningsRequest>()))
                .Throws(new ArgumentException("Invalid input"));
            _sut = new HandleRecalculateEarningsRequest(_mockService.Object);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/recalculate-earnings")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _sut.RunHttp(request) as ContentResult;

            // Assert
            response.Should().NotBeNull();
            response.Content.Should().Contain("Example");
        }

        [Test]
        public async Task Then_an_error_response_is_returned_when_a_RecalculateEarningsServiceException_is_thrown()
        {
            // Arrange
            _mockService.Setup(x => x.RecalculateEarnings(It.IsAny<RecalculateEarningsRequest>()))
                .Throws(new RecalculateEarningsServiceException(HttpStatusCode.InternalServerError, "System error"));
            _sut = new HandleRecalculateEarningsRequest(_mockService.Object);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/recalculate-earnings")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _sut.RunHttp(request) as ContentResult;

            // Assert
            response.Should().NotBeNull();
            response.Content.Should().Contain("System error");
        }

        [Test]
        public async Task Then_an_error_response_is_returned_when_an_unexpected_error_is_thrown()
        {
            // Arrange
            _mockService.Setup(x => x.RecalculateEarnings(It.IsAny<RecalculateEarningsRequest>()))
                .Throws(new Exception("Unexpected error"));
            _sut = new HandleRecalculateEarningsRequest(_mockService.Object);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/recalculate-earnings")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _sut.RunHttp(request) as ContentResult;

            // Assert
            response.Should().NotBeNull();
            response.Content.Should().Contain("Unexpected error");
        }
    }
}
