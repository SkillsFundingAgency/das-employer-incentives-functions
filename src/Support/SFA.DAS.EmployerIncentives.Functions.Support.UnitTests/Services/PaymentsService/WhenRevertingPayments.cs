using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Payments;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Payments.Types;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.UnitTests.Services.PaymentsService
{
    [TestFixture]
    public class WhenRevertingPayments
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

            _sut = new PaymentsServiceValidation(new Support.Services.Payments.PaymentsService(_testClient));
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client()
        {
            // Arrange
            var revertPaymentsRequest = _fixture.Create<RevertPaymentsRequest>();

            // Act
            await _sut.RevertPayments(revertPaymentsRequest);

            // Assert
            _testClient.VerifyPostAsAsync($"revert-payments", revertPaymentsRequest, Times.Once());
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_client_returns_bad_request()
        {
            // Arrange
            var revertPaymentsRequest = _fixture.Create<RevertPaymentsRequest>();
            _testClient.SetUpPostAsAsync(HttpStatusCode.BadRequest);
            
            // Act
            Func<Task> result = async () => await _sut.RevertPayments(revertPaymentsRequest);

            // Assert
            result.Should().Throw<PaymentsServiceException>();
        }

        [Test]
        public void Then_an_exception_is_thrown_if_the_payment_ids_are_not_set()
        {
            // Arrange
            var revertPaymentsRequest = _fixture.Build<RevertPaymentsRequest>()
                .With(x => x.Payments, new List<Guid>())
                .Create();
            _testClient.SetUpPostAsAsync(HttpStatusCode.BadRequest);

            // Act
            Func<Task> result = async () => await _sut.RevertPayments(revertPaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Payment Ids are not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_if_the_payment_ids_are_not_supplied()
        {
            // Arrange
            var revertPaymentsRequest = _fixture.Build<RevertPaymentsRequest>()
                .Without(x => x.Payments)
                .Create();
            _testClient.SetUpPostAsAsync(HttpStatusCode.BadRequest);

            // Act
            Func<Task> result = async () => await _sut.RevertPayments(revertPaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Payment Ids are not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_is_not_set()
        {
            // Arrange
            var revertPaymentsRequest = _fixture.Build<RevertPaymentsRequest>()
                .Without(x => x.ServiceRequest)
                .Create();
            _testClient.SetUpPostAsAsync(HttpStatusCode.BadRequest);

            // Act
            Func<Task> result = async () => await _sut.RevertPayments(revertPaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request is not set*");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_TaskId_is_not_set(string value)
        {
            // Arrange
            var revertPaymentsRequest = _fixture.Create<RevertPaymentsRequest>();
            revertPaymentsRequest.ServiceRequest.TaskId = value;

            // Act
            Func<Task> result = async () => await _sut.RevertPayments(revertPaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request Task Id is not set*");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_DecisionReference_is_not_set(string value)
        {
            // Arrange
            var revertPaymentsRequest = _fixture.Create<RevertPaymentsRequest>();
            revertPaymentsRequest.ServiceRequest.DecisionReference = value;

            // Act
            Func<Task> result = async () => await _sut.RevertPayments(revertPaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request Decision Reference is not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_TaskCreatedDate_is_not_set()
        {
            // Arrange
            var revertPaymentsRequest = _fixture.Build<RevertPaymentsRequest>()
                .With(x => x.ServiceRequest, 
                    _fixture.Build<ServiceRequest>().Without(x => x.TaskCreatedDate).Create())
                .Create();
            
            // Act
            Func<Task> result = async () => await _sut.RevertPayments(revertPaymentsRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request Task Created Date is not set*");
        }
    }
}
