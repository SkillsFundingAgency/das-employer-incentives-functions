using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs;

namespace SFA.DAS.EmployerIncentives.Functions.Support.UnitTests.Functions
{
    public class WhenHandleTriggerPaymentApprovalRequest
    {
        private HandleTriggerPaymentApproval _sut;
        private Mock<IJobsService> _mockJobsService;

        [SetUp]
        public void Setup()
        {
            _mockJobsService = new Mock<IJobsService>();

            _mockJobsService
                .Setup(m => m.TriggerPaymentApproval())
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            _sut = new HandleTriggerPaymentApproval(_mockJobsService.Object);
        }

        [Test]
        public async Task Then_a_PaymentApproval_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var request = TestHttpClient.CreateHttpRequest("");

            // Act
            await _sut.RunHttp(request);

            // Assert
            _mockJobsService
                .Verify(m => m.TriggerPaymentApproval()
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