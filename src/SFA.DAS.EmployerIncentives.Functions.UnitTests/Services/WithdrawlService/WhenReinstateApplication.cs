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
    public class WhenReinstateApplication
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
            var reinstateApplicationRequest = _fixture.Create<ReinstateApplicationRequest>();

            // Act
            await _sut.Reinstate(reinstateApplicationRequest);

            // Assert
            _testClient.VerifyPostAsAsync($"withdrawal-reinstatements", reinstateApplicationRequest, Times.Once());
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_AccountLegalEntityId_is_not_set()
        {
            // Arrange
            var reinstateApplicationRequest = _fixture.Create<ReinstateApplicationRequest>(); 
            reinstateApplicationRequest.AccountLegalEntityId = default;

            // Act
            Func<Task> result = async () => await _sut.Reinstate(reinstateApplicationRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("AccountLegalEntityId not set (Parameter 'AccountLegalEntityId')");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ULN_is_not_set()
        {
            // Arrange
            var reinstateApplicationRequest = _fixture.Create<ReinstateApplicationRequest>();
            reinstateApplicationRequest.ULN = default;

            // Act
            Func<Task> result = async () => await _sut.Reinstate(reinstateApplicationRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ULN not set (Parameter 'ULN')");
        }
    }
}