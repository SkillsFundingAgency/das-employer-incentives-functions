using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.UnitTests.Services.WithdrawService
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
            var application = _fixture.Create<Application>();
            var reinstateApplicationRequest = _fixture.Create<ReinstateApplicationRequest>(); 
            reinstateApplicationRequest.Applications = new[] { application };
            application.AccountLegalEntityId = default;
            var service = new WithdrawServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Reinstate(reinstateApplicationRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("AccountLegalEntityId not set (Parameter 'AccountLegalEntityId')");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ULN_is_not_set()
        {
            // Arrange
            var application = _fixture.Create<Application>();
            var reinstateApplicationRequest = _fixture.Create<ReinstateApplicationRequest>();
            reinstateApplicationRequest.Applications = new[] { application };
            application.ULN = default;
            var service = new WithdrawServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Reinstate(reinstateApplicationRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ULN not set (Parameter 'ULN')");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_is_not_set()
        {
            // Arrange
            var reinstateApplicationRequest = _fixture.Create<ReinstateApplicationRequest>();
            reinstateApplicationRequest.Applications.First().ServiceRequest = default;
            var service = new WithdrawServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Reinstate(reinstateApplicationRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request is not set*");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_TaskId_is_not_set(string value)
        {
            // Arrange
            var reinstateApplicationRequest = _fixture.Create<ReinstateApplicationRequest>();
            reinstateApplicationRequest.Applications.First().ServiceRequest.TaskId = value;
            var service = new WithdrawServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Reinstate(reinstateApplicationRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request Task Id is not set*");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_DecisionReference_is_not_set(string value)
        {
            // Arrange
            var reinstateApplicationRequest = _fixture.Create<ReinstateApplicationRequest>();
            reinstateApplicationRequest.Applications.First().ServiceRequest.DecisionReference = value;
            var service = new WithdrawServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Reinstate(reinstateApplicationRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request Decision Reference is not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_TaskCreatedDate_is_not_set()
        {
            // Arrange
            var reinstateApplicationRequest = _fixture.Create<ReinstateApplicationRequest>();
            reinstateApplicationRequest.Applications.First().ServiceRequest.TaskCreatedDate = default;
            var service = new WithdrawServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Reinstate(reinstateApplicationRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request Task Created Date is not set*");
        }
    }
}