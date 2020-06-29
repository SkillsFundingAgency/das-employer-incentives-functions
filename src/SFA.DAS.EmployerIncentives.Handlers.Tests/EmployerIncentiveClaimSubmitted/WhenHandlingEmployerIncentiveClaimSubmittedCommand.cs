using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.Commands.EmployerIncentiveClaimSubmitted;
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

        [SetUp]
        public void Arrange()
        {
            _apiClient = new Mock<ICalculatePaymentApiClient>();
            _handler = new EmployerIncentiveClaimSubmittedCommandHandler(_apiClient.Object);
        }

        [Test]
        public async Task Then_calculation_of_first_payment_is_requested()
        {
            var incentiveClaimApprenticeshipId = Guid.NewGuid();

            await _handler.Handle(new EmployerIncentiveClaimSubmittedCommand(incentiveClaimApprenticeshipId));

            _apiClient.Verify(x => x.CalculateFirstPayment(incentiveClaimApprenticeshipId), Times.Once);
        }
    }
}
