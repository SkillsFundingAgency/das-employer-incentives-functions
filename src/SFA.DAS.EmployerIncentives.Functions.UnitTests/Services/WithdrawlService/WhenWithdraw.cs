using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals.Types;
using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;

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
            var withdrawRequest = _fixture.Build<WithdrawRequest>().With(r => r.Type, WithdrawalType.Employer).Create();

            // Act
            await _sut.Withdraw(withdrawRequest);

            // Assert
            _testClient.VerifyPostAsAsync($"withdrawals", withdrawRequest, Times.Once());
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_AccountLegalEntityId_is_not_set()
        {
            // Arrange
            var withdrawRequest = _fixture.Build<WithdrawRequest>().With(r => r.Type, WithdrawalType.Employer).Create();
            withdrawRequest.AccountLegalEntityId = default;

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
            withdrawRequest.Type = default;

            // Act
            Func<Task> result = async () => await _sut.Withdraw(withdrawRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Type not set or invalid (Parameter 'Type')");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ULN_is_not_set()
        {
            // Arrange
            var withdrawRequest = _fixture.Build<WithdrawRequest>().With(r => r.Type, WithdrawalType.Employer).Create();
            withdrawRequest.ULN = default;

            // Act
            Func<Task> result = async () => await _sut.Withdraw(withdrawRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ULN not set (Parameter 'ULN')");
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client_with_the_task_date_set_when_service_request_not_set()
        {
            // Arrange
            var withdrawRequest = _fixture.Build<WithdrawRequest>().With(r => r.Type, WithdrawalType.Employer).Create();
            withdrawRequest.ServiceRequest = null;

            // Act
            await _sut.Withdraw(withdrawRequest);

            // Assert
            _testClient.VerifyPostAsAsync($"withdrawals", withdrawRequest, Times.Once());
        }
    }
}