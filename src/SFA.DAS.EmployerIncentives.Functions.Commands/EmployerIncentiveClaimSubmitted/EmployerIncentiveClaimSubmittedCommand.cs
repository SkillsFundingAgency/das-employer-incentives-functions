using SFA.DAS.EmployerIncentives.Infrastructure.Commands;
using System;

namespace SFA.DAS.EmployerIncentives.Functions.Commands.EmployerIncentiveClaimSubmitted
{
    public class EmployerIncentiveClaimSubmittedCommand : ICommand
    {
        public Guid IncentiveClaimApprenticeshipId { get; private set; }

        public EmployerIncentiveClaimSubmittedCommand(Guid claimId)
        {
            IncentiveClaimApprenticeshipId = claimId;
        }
    }
}
