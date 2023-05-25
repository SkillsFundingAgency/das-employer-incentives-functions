using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs;

namespace SFA.DAS.EmployerIncentives.Functions.Support.UnitTests.Functions
{
    public class WhenHandleTriggerPaymentValidationRequest
    {
        private HandleTriggerPaymentValidation _sut;
        private Mock<IJobsService> _mockJobsService;

        [SetUp]
        public void Setup()
        {
            _mockJobsService = new Mock<IJobsService>();

            _mockJobsService
                .Setup(m => m.TriggerPaymentValidation())
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            _sut = new HandleTriggerPaymentValidation(_mockJobsService.Object);
        }

        [Test]
        public async Task Then_a_PaymentValidation_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var request = TestHttpClient.CreateHttpRequest("");

            // Act
            await _sut.RunHttp(request);

            // Assert
            _mockJobsService
                .Verify(m => m.TriggerPaymentValidation()
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