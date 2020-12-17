using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.PausePaymentsService
{
    public class WhenSettingPauseStatus
    {
        private Functions.LegalEntities.Services.PausePayments.PausePaymentsService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _baseAddress = new Uri(@"http://localhost");
            _testClient = new TestHttpClient(_baseAddress);

            _testClient.SetUpPostAsAsync(HttpStatusCode.OK);

            _sut = new Functions.LegalEntities.Services.PausePayments.PausePaymentsService(_testClient);
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client()
        {
            // Arrange
            var pausePaymentsRequest = BuildValidPausePaymentsRequest();

            // Act
            await _sut.SetPauseStatus(pausePaymentsRequest);

            // Assert
            _testClient.VerifyPostAsAsync($"pause-payments", pausePaymentsRequest, Times.Once());
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_AccountLegalEntityId_is_not_set()
        {
            // Arrange
            var pausePaymentsRequest = BuildValidPausePaymentsRequest();
            pausePaymentsRequest.AccountLegalEntityId = default;

            // Act
            Func<Task> result = async () => await _sut.SetPauseStatus(pausePaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("AccountLegalEntityId is not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ULN_is_not_set()
        {
            // Arrange
            var pausePaymentsRequest = BuildValidPausePaymentsRequest();
            pausePaymentsRequest.ULN = default;

            // Act
            Func<Task> result = async () => await _sut.SetPauseStatus(pausePaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ULN is not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_Action_is_not_set()
        {
            // Arrange
            var pausePaymentsRequest = BuildValidPausePaymentsRequest();
            pausePaymentsRequest.Action = default;

            // Act
            Func<Task> result = async () => await _sut.SetPauseStatus(pausePaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Action is not set or invalid*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_is_not_set()
        {
            // Arrange
            var pausePaymentsRequest = BuildValidPausePaymentsRequest();
            pausePaymentsRequest.ServiceRequest = default;

            // Act
            Func<Task> result = async () => await _sut.SetPauseStatus(pausePaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request is not set*");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_TaskId_is_not_set(string value)
        {
            // Arrange
            var pausePaymentsRequest = BuildValidPausePaymentsRequest();
            pausePaymentsRequest.ServiceRequest.TaskId = value;

            // Act
            Func<Task> result = async () => await _sut.SetPauseStatus(pausePaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request Task Id is not set*");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_DecisionReference_is_not_set(string value)
        {
            // Arrange
            var pausePaymentsRequest = BuildValidPausePaymentsRequest();
            pausePaymentsRequest.ServiceRequest.DecisionReference = value;

            // Act
            Func<Task> result = async () => await _sut.SetPauseStatus(pausePaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request Decision Reference is not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_TaskCreatedDate_is_not_set()
        {
            // Arrange
            var pausePaymentsRequest = BuildValidPausePaymentsRequest();
            pausePaymentsRequest.ServiceRequest.TaskCreatedDate = default;

            // Act
            Func<Task> result = async () => await _sut.SetPauseStatus(pausePaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request Task Created Date is not set*");
        }

        private PausePaymentsRequest BuildValidPausePaymentsRequest()
        {
            return _fixture.Build<PausePaymentsRequest>().With(r => r.Action, PausePaymentsAction.Pause).Create();

        }
    }
}