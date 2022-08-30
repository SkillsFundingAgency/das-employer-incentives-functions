using System;
using System.Linq;
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
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Types;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    public class WhenHandleRefreshEmploymentCheckRequest
    {
        private HandleRefreshEmploymentCheckRequest _sut;
        private Mock<IEmploymentCheckService> _mockEmploymentCheckService;
        private Fixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();
            _mockEmploymentCheckService = new Mock<IEmploymentCheckService>();

            _sut = new HandleRefreshEmploymentCheckRequest(_mockEmploymentCheckService.Object);
        }

        [Test]
        public async Task Then_a_request_is_sent_to_the_employer_incentives_API()
        {
            // Arrange
            var refreshEmploymentCheckRequest = new EmploymentCheckRequest
            {
                CheckType = RefreshEmploymentCheckType.InitialEmploymentChecks.ToString(),
                Applications = _fixture.CreateMany<Application>(3).ToArray(),
                ServiceRequest = _fixture.Create<ServiceRequest>()
            };

            _mockEmploymentCheckService.Setup(x => x.Refresh(It.IsAny<EmploymentCheckRequest>()))
                .Returns(Task.CompletedTask);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/HttpTriggerEmploymentCheckRequest")
            {
                Content = new StringContent(JsonConvert.SerializeObject(refreshEmploymentCheckRequest), Encoding.UTF8, "application/json")
            };

            // Act
            var result = await _sut.RunHttp(request) as OkResult;

            // Assert
            _mockEmploymentCheckService.Verify(m => m.Refresh(It.IsAny<EmploymentCheckRequest>()), Times.Once);
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Then_a_bad_request_response_is_returned_on_error_response_from_API()
        {
            // Arrange
            var refreshEmploymentCheckRequest = new EmploymentCheckRequest
            {
                CheckType = RefreshEmploymentCheckType.InitialEmploymentChecks.ToString(),
                Applications = _fixture.CreateMany<Application>(3).ToArray(),
                ServiceRequest = _fixture.Create<ServiceRequest>()
            };

            _mockEmploymentCheckService.Setup(x => x.Refresh(It.IsAny<EmploymentCheckRequest>()))
                .Throws(new EmploymentCheckServiceException(HttpStatusCode.BadRequest, "Bad request"));

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/HttpTriggerEmploymentCheckRequest")
            {
                Content = new StringContent(JsonConvert.SerializeObject(refreshEmploymentCheckRequest), Encoding.UTF8, "application/json")
            };

            // Act
            var result = await _sut.RunHttp(request) as ContentResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            result.Content.Should().Contain("Bad request");
        }

        [Test]
        public async Task Then_a_bad_request_response_is_returned_on_input_validation_error()
        {
            // Arrange
            var refreshEmploymentCheckRequest = new EmploymentCheckRequest
            {
                CheckType = RefreshEmploymentCheckType.InitialEmploymentChecks.ToString(),
                Applications = _fixture.CreateMany<Application>(3).ToArray(),
                ServiceRequest = _fixture.Create<ServiceRequest>()
            };

            _mockEmploymentCheckService.Setup(x => x.Refresh(It.IsAny<EmploymentCheckRequest>()))
                .Throws(new ArgumentException("Invalid parameter"));

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/HttpTriggerEmploymentCheckRequest")
            {
                Content = new StringContent(JsonConvert.SerializeObject(refreshEmploymentCheckRequest), Encoding.UTF8, "application/json")
            };

            // Act
            var result = await _sut.RunHttp(request) as ContentResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            result.Content.Should().Contain("Invalid parameter");
        }

        [Test]
        public async Task Then_an_error_response_is_returned_for_unhandled_errors()
        {
            // Arrange
            var refreshEmploymentCheckRequest = new EmploymentCheckRequest
            {
                CheckType = RefreshEmploymentCheckType.InitialEmploymentChecks.ToString(),
                Applications = _fixture.CreateMany<Application>(3).ToArray(),
                ServiceRequest = _fixture.Create<ServiceRequest>()
            };

            _mockEmploymentCheckService.Setup(x => x.Refresh(It.IsAny<EmploymentCheckRequest>()))
                .Throws(new Exception("System error"));

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/HttpTriggerEmploymentCheckRequest")
            {
                Content = new StringContent(JsonConvert.SerializeObject(refreshEmploymentCheckRequest), Encoding.UTF8, "application/json")
            };

            // Act
            var result = await _sut.RunHttp(request) as ContentResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Content.Should().Contain("System error");
        }
    }
}
