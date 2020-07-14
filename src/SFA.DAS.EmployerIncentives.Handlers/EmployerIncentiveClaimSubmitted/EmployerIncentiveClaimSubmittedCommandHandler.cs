using SFA.DAS.EmployerIncentives.Functions.Commands.EmployerIncentiveClaimSubmitted;
using SFA.DAS.EmployerIncentives.Handlers.Exceptions;
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
            var success = await ApiClient.CalculateFirstPayment(command.AccountId, command.IncentiveClaimApprenticeshipId);

            if (!success)
            {
                throw new CommandFailureException($"Payment calculation request failed for account {command.AccountId} claim {command.IncentiveClaimApprenticeshipId}");
            }
        }
    }
}
