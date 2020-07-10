using SFA.DAS.EmployerIncentives.Infrastructure.Commands;
using System;

namespace SFA.DAS.EmployerIncentives.Functions.Commands.EmployerIncentiveClaimSubmitted
{
    public class EmployerIncentiveClaimSubmittedCommand : ICommand
    {
        public long AccountId { get; private set; }
        public Guid IncentiveClaimApprenticeshipId { get; private set; }

        public EmployerIncentiveClaimSubmittedCommand(long accountId, Guid claimId)
        {
            AccountId = accountId;
            IncentiveClaimApprenticeshipId = claimId;
        }
    }
}
