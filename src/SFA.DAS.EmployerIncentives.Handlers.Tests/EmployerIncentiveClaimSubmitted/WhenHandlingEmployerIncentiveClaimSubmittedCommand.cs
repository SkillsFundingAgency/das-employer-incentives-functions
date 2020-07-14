using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.Commands.EmployerIncentiveClaimSubmitted;
using SFA.DAS.EmployerIncentives.Handlers.Exceptions;
using SFA.DAS.EmployerIncentives.Infrastructure.ApiClient;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Handlers.Tests.EmployerIncentiveClaimSubmitted
{
    [TestFixture]
    public class WhenHandlingEmployerIncentiveClaimSubmittedCommand
    {
        private Mock<ICalculatePaymentApiClient> _apiClient;
        private EmployerIncentiveClaimSubmittedCommandHandler _handler;
        private long _accountId;
        private Guid _claimId = Guid.NewGuid();

        [SetUp]
        public void Arrange()
        {
            _accountId = 1234;
            _claimId = Guid.NewGuid();

            _apiClient = new Mock<ICalculatePaymentApiClient>();
            _apiClient.Setup(x => x.CalculateFirstPayment(_accountId, _claimId)).ReturnsAsync(true);

            _handler = new EmployerIncentiveClaimSubmittedCommandHandler(_apiClient.Object);
        }

        [Test]
        public async Task Then_calculation_of_payment_is_requested()
        {
            // Act
            await _handler.Handle(new EmployerIncentiveClaimSubmittedCommand(_accountId, _claimId));

            // Assert
            _apiClient.Verify(x => x.CalculateFirstPayment(_accountId, _claimId), Times.Once);
        }

        [Test]
        public async Task Then_calculation_results_in_an_error()
        {
            // Arrange
            _apiClient.Setup(x => x.CalculateFirstPayment(_accountId, _claimId)).ReturnsAsync(false);

            // Act/Assert
            Assert.Throws<CommandFailureException>(() => _handler.Handle(new EmployerIncentiveClaimSubmittedCommand(_accountId, _claimId)).GetAwaiter().GetResult());
        }
    }
}
