using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals.Types;
using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.WithdrawService
{
    public class WhenWithdraw
    {
        private WithdrawalService _sut;
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

            _sut = new WithdrawalService(_testClient);
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client()
        {
            // Arrange
            var withdrawRequest = _fixture.Build<WithdrawRequest>().With(r => r.WithdrawalType, WithdrawalType.Employer).Create();

            // Act
            await _sut.Withdraw(withdrawRequest);

            // Assert
            _testClient.VerifyPostAsAsync($"withdrawals", withdrawRequest, Times.Once());
        }

        [Test]
        public void Then_an_exception_is_thrown_if_the_applications_are_not_set() 
        {
            // Arrange
            var withdrawRequest = _fixture.Build<WithdrawRequest>().With(r => r.WithdrawalType, WithdrawalType.Compliance).Create();
            withdrawRequest.Applications = default;

            // Act
            Func<Task> result = async () => await _sut.Withdraw(withdrawRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Applications are not set (Parameter 'applications')");
        }

        [Test]
        public void Then_an_exception_is_thrown_if_the_applications_are_empty()
        {
            // Arrange
            var withdrawRequest = _fixture.Build<WithdrawRequest>().With(r => r.WithdrawalType, WithdrawalType.Compliance).Create();
            withdrawRequest.Applications = Array.Empty<Application>();

            // Act
            Func<Task> result = async () => await _sut.Withdraw(withdrawRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Applications are not set (Parameter 'applications')");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_AccountLegalEntityId_is_not_set()
        {
            // Arrange
            var withdrawRequest = _fixture.Build<WithdrawRequest>().With(r => r.WithdrawalType, WithdrawalType.Employer).Create();
            withdrawRequest.Applications = new []
            {
                new Application { AccountLegalEntityId = default, ULN = _fixture.Create<long>() }
            };

            // Act
            Func<Task> result = async () => await _sut.Withdraw(withdrawRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("AccountLegalEntityId not set (Parameter 'AccountLegalEntityId')");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_Type_is_not_set()
        {
            // Arrange
            var withdrawRequest = _fixture.Create<WithdrawRequest>();
            withdrawRequest.WithdrawalType = default;

            // Act
            Func<Task> result = async () => await _sut.Withdraw(withdrawRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("WithdrawalType not set or invalid (Parameter 'WithdrawalType')");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ULN_is_not_set()
        {
            // Arrange
            var withdrawRequest = _fixture.Build<WithdrawRequest>().With(r => r.WithdrawalType, WithdrawalType.Employer).Create();
            withdrawRequest.Applications = new[]
            {
                new Application { AccountLegalEntityId = _fixture.Create<long>(), ULN = default }
            };

            // Act
            Func<Task> result = async () => await _sut.Withdraw(withdrawRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ULN not set (Parameter 'ULN')");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_service_request_is_not_set()
        {
            // Arrange
            var withdrawRequest = _fixture.Build<WithdrawRequest>()
                .With(r => r.WithdrawalType, WithdrawalType.Compliance)
                .Without(r => r.ServiceRequest)
                .Create();
            
            // Act
            Func<Task> result = async () => await _sut.Withdraw(withdrawRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ServiceRequest not set (Parameter 'ServiceRequest')");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_service_request_task_id_is_not_set()
        {
            // Arrange
            var withdrawRequest = _fixture.Build<WithdrawRequest>()
                .With(r => r.WithdrawalType, WithdrawalType.Compliance)
                .With(r => r.ServiceRequest, _fixture.Build<ServiceRequest>().Without(x => x.TaskId).Create())
                .Create();

            // Act
            Func<Task> result = async () => await _sut.Withdraw(withdrawRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ServiceRequest TaskId not set (Parameter 'TaskId')");
        }
        
        [Test]
        public void Then_an_exception_is_thrown_when_the_service_request_decision_reference_is_not_set()
        {
            // Arrange
            var withdrawRequest = _fixture.Build<WithdrawRequest>()
                .With(r => r.WithdrawalType, WithdrawalType.Compliance)
                .With(r => r.ServiceRequest, _fixture.Build<ServiceRequest>().Without(x => x.DecisionReference).Create())
                .Create();

            // Act
            Func<Task> result = async () => await _sut.Withdraw(withdrawRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ServiceRequest DecisionReference not set (Parameter 'DecisionReference')");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_service_request_date_is_not_set()
        {
            // Arrange
            var withdrawRequest = _fixture.Build<WithdrawRequest>()
                .With(r => r.WithdrawalType, WithdrawalType.Compliance)
                .With(r => r.ServiceRequest, _fixture.Build<ServiceRequest>().Without(x => x.TaskCreatedDate).Create())
                .Create();

            // Act
            Func<Task> result = async () => await _sut.Withdraw(withdrawRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ServiceRequest TaskCreatedDate not set (Parameter 'TaskCreatedDate')");
        }
    }
}