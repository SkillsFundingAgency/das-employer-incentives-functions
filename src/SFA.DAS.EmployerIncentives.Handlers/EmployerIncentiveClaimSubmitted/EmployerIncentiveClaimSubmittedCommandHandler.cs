using SFA.DAS.EmployerIncentives.Functions.Commands.EmployerIncentiveClaimSubmitted;
using SFA.DAS.EmployerIncentives.Infrastructure.ApiClient;
using SFA.DAS.EmployerIncentives.Infrastructure.Commands;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Handlers
{
    public class EmployerIncentiveClaimSubmittedCommandHandler : ICommandHandler<EmployerIncentiveClaimSubmittedCommand>
    {
        private readonly ICalculatePaymentApiClient ApiClient;

        public EmployerIncentiveClaimSubmittedCommandHandler(ICalculatePaymentApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public async Task Handle(EmployerIncentiveClaimSubmittedCommand command)
        {
            await ApiClient.CalculateFirstPayment(command.ClaimId);
        }
    }
}
