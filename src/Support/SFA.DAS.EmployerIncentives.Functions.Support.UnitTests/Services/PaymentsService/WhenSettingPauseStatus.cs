using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Types;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Payments;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Payments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.UnitTests.Services.PaymentsService
{
    public class WhenSettingPauseStatus
    {
        private IPaymentsService _sut;
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

            _sut = new PaymentsServiceValidation(
                new Support.Services.Payments.PaymentsService(_testClient)
                );
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

        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.BadRequest)]
        public void Then_a_PausePaymentServiceException_is_thrown_when_the_client_returns(HttpStatusCode clientResponse)
        {
            // Arrange
            var pausePaymentsRequest = BuildValidPausePaymentsRequest();
            _testClient.SetUpPostAsAsync(clientResponse);

            // Act
            Func<Task> result = async () => await _sut.SetPauseStatus(pausePaymentsRequest);

            // Assert
            result.Should().Throw<PaymentsServiceException>();
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_AccountLegalEntityId_is_not_set()
        {
            // Arrange
            var pausePaymentsRequest = BuildValidPausePaymentsRequest();
            pausePaymentsRequest.Applications = new List<Application>()
            {
                _fixture.Create<Application>(),
                new Application()
                {
                    AccountLegalEntityId = default,
                    ULN = 1234
                },
                _fixture.Create<Application>()
            }.ToArray();

            // Act
            Func<Task> result = async () => await _sut.SetPauseStatus(pausePaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("AccountLegalEntityId not set for AccountLegalEntityId : 0, ULN : 1234");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ULN_is_not_set()
        {
            // Arrange
            var pausePaymentsRequest = BuildValidPausePaymentsRequest();
            
            pausePaymentsRequest.Applications = new List<Application>()
            {
                _fixture.Create<Application>(),
                new Application()
                {
                    AccountLegalEntityId = 1234,
                    ULN = default
                },
                _fixture.Create<Application>()
            }.ToArray();

            // Act
            Func<Task> result = async () => await _sut.SetPauseStatus(pausePaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ULN not set for AccountLegalEntityId : 1234, ULN : 0");
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

        [Test]
        public void Then_an_exception_is_thrown_when_the_AccountLegalEntityId_ULN_combination_is_duplicated()
        {
            // Arrange
            var pausePaymentsRequest = BuildValidPausePaymentsRequest();

            pausePaymentsRequest.Applications = new List<Application>()
            {
                _fixture.Create<Application>(),
                new Application()
                {
                    AccountLegalEntityId = 1234,
                    ULN = 1234
                },
                new Application()
                {
                    AccountLegalEntityId = 1234,
                    ULN = 1234
                }
            }.ToArray();

            // Act
            Func<Task> result = async () => await _sut.SetPauseStatus(pausePaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Duplicate application entries exist. The combination of AccountLegalEntityId and ULN should be unique.");
        }

        private PausePaymentsRequest BuildValidPausePaymentsRequest()
        {
            return _fixture.Build<PausePaymentsRequest>().With(r => r.Action, PausePaymentsAction.Pause).Create();

        }
    }
}