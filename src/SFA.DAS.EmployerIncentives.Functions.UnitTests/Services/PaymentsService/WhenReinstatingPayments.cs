using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments.Types;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.PaymentsService
{
    [TestFixture]
    public class WhenReinstatingPayments
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

            _sut = new PaymentsServiceValidation(new Functions.LegalEntities.Services.Payments.PaymentsService(_testClient));
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client()
        {
            // Arrange
            var reinstatePaymentsRequest = _fixture.Create<ReinstatePaymentsRequest>();

            // Act
            await _sut.ReinstatePayments(reinstatePaymentsRequest);

            // Assert
            _testClient.VerifyPostAsAsync($"reinstate-payments", reinstatePaymentsRequest, Times.Once());
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_client_returns_bad_request()
        {
            // Arrange
            var reinstatePaymentsRequest = _fixture.Create<ReinstatePaymentsRequest>();
            _testClient.SetUpPostAsAsync(HttpStatusCode.BadRequest);

            // Act
            Func<Task> result = async () => await _sut.ReinstatePayments(reinstatePaymentsRequest);

            // Assert
            result.Should().ThrowAsync<PaymentsServiceException>();
        }

        [Test]
        public void Then_an_exception_is_thrown_if_the_payment_ids_are_not_set()
        {
            // Arrange
            var reinstatePaymentsRequest = _fixture.Build<ReinstatePaymentsRequest>()
                .With(x => x.Payments, new List<Guid>())
                .Create();
            _testClient.SetUpPostAsAsync(HttpStatusCode.BadRequest);

            // Act
            Func<Task> result = async () => await _sut.ReinstatePayments(reinstatePaymentsRequest);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage("Payment Ids are not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_if_the_payment_ids_are_not_supplied()
        {
            // Arrange
            var reinstatePaymentsRequest = _fixture.Build<ReinstatePaymentsRequest>()
                .Without(x => x.Payments)
                .Create();
            _testClient.SetUpPostAsAsync(HttpStatusCode.BadRequest);

            // Act
            Func<Task> result = async () => await _sut.ReinstatePayments(reinstatePaymentsRequest);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage("Payment Ids are not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_is_not_set()
        {
            // Arrange
            var reinstatePaymentsRequest = _fixture.Build<ReinstatePaymentsRequest>()
                .Without(x => x.ServiceRequest)
                .Create();
            _testClient.SetUpPostAsAsync(HttpStatusCode.BadRequest);

            // Act
            Func<Task> result = async () => await _sut.ReinstatePayments(reinstatePaymentsRequest);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage("Service Request is not set*");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_TaskId_is_not_set(string value)
        {
            // Arrange
            var reinstatePaymentsRequest = _fixture.Create<ReinstatePaymentsRequest>();
            reinstatePaymentsRequest.ServiceRequest.TaskId = value;

            // Act
            Func<Task> result = async () => await _sut.ReinstatePayments(reinstatePaymentsRequest);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage("Service Request Task Id is not set*");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_DecisionReference_is_not_set(string value)
        {
            // Arrange
            var reinstatePaymentsRequest = _fixture.Create<ReinstatePaymentsRequest>();
            reinstatePaymentsRequest.ServiceRequest.DecisionReference = value;

            // Act
            Func<Task> result = async () => await _sut.ReinstatePayments(reinstatePaymentsRequest);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage("Service Request Decision Reference is not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_TaskCreatedDate_is_not_set()
        {
            // Arrange
            var reinstatePaymentsRequest = _fixture.Build<ReinstatePaymentsRequest>()
                .With(x => x.ServiceRequest,
                    _fixture.Build<ReinstatePaymentsServiceRequest>().Without(x => x.TaskCreatedDate).Create())
                .Create();

            // Act
            Func<Task> result = async () => await _sut.ReinstatePayments(reinstatePaymentsRequest);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage("Service Request Task Created Date is not set*");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Then_an_exception_is_thrown_when_the_ServiceRequest_Process_is_not_set(string value)
        {
            // Arrange
            var reinstatePaymentsRequest = _fixture.Create<ReinstatePaymentsRequest>();
            reinstatePaymentsRequest.ServiceRequest.Process = value;

            // Act
            Func<Task> result = async () => await _sut.ReinstatePayments(reinstatePaymentsRequest);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage("Service Request Process is not set*");
        }
    }
}
