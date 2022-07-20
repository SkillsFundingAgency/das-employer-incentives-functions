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
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    [TestFixture]
    public class WhenHandleReinstatePaymentsRequest
    {
        private HandleReinstatePaymentsRequest _sut;
        private Mock<IPaymentsService> _service;
        private Fixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();
            _service = new Mock<IPaymentsService>();

            _service.Setup(m => m.ReinstatePayments(It.IsAny<ReinstatePaymentsRequest>()))
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            _sut = new HandleReinstatePaymentsRequest(_service.Object);
        }

        [Test]
        public async Task Then_a_reinstate_payments_request_is_sent_to_the_service()
        {
            // Arrange
            var requestMessage = _fixture.Create<ReinstatePaymentsRequest>();
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestMessage), Encoding.UTF8, "application/json")
            };

            ReinstatePaymentsRequest actualRequest = null;
            _service.Setup(x => x.ReinstatePayments(It.IsAny<ReinstatePaymentsRequest>()))
                .Callback<ReinstatePaymentsRequest>(x => actualRequest = x);

            // Act
            await _sut.RunHttp(request);

            // Assert
            actualRequest.Should().BeEquivalentTo(requestMessage);
        }

        [Test]
        public async Task Then_a_BadRequest_response_is_returned_on_validation_failure()
        {
            // Arrange
            var requestMessage = _fixture.Create<ReinstatePaymentsRequest>();
            var request = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestMessage), Encoding.UTF8, "application/json")
            };
            
            _service.Setup(x => x.ReinstatePayments(It.IsAny<ReinstatePaymentsRequest>()))
                .Throws(new ArgumentException("Request invalid"));

            // Act
            var response = await _sut.RunHttp(request) as ContentResult;

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
