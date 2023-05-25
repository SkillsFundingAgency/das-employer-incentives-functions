using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Payments;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Payments.Types;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.UnitTests.Functions
{
    [TestFixture]
    public class WhenHandleRevertPaymentsRequest
    {
        private HandleRevertPaymentsRequest _sut;
        private Mock<IPaymentsService> _service;
        private Fixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();
            _service = new Mock<IPaymentsService>();

            _service.Setup(m => m.RevertPayments(It.IsAny<RevertPaymentsRequest>()))
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            _sut = new HandleRevertPaymentsRequest(_service.Object);
        }

        [Test]
        public async Task Then_a_revert_payments_request_is_sent_to_the_service()
        {
            // Arrange
            var requestMessage = _fixture.Create<RevertPaymentsRequest>();
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestMessage), Encoding.UTF8, "application/json")
            };

            RevertPaymentsRequest actualRequest = null;
            _service.Setup(x => x.RevertPayments(It.IsAny<RevertPaymentsRequest>()))
                .Callback<RevertPaymentsRequest>(x => actualRequest = x);

            // Act
            await _sut.RunHttp(request);

            // Assert
            actualRequest.Should().BeEquivalentTo(requestMessage);
        }

        [Test]
        public async Task Then_an_Ok_response_is_returned_on_success()
        {
            // Arrange
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(_fixture.Create<ReinstateApplicationRequest>()), Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _sut.RunHttp(request) as OkResult;

            // Assert
            response.Should().NotBeNull();
        }

        [Test]
        public async Task Then_a_BadRequest_response_is_returned_on_validation_failure()
        {
            // Arrange
            var requestMessage = _fixture.Create<RevertPaymentsRequest>();
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestMessage), Encoding.UTF8, "application/json")
            };

            const string errorMessage = "Request invalid";
            _service.Setup(x => x.RevertPayments(It.IsAny<RevertPaymentsRequest>()))
                .Throws(new ArgumentException(errorMessage));

            // Act
            var response = await _sut.RunHttp(request) as ContentResult;

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
