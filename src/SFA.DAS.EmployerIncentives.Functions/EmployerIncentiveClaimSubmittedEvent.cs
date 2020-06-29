using SFA.DAS.NServiceBus;
using System;

namespace SFA.DAS.EmployerIncentives.Functions
{
    // TODO: move this to SFA.DAS.EmployerAccounts.Messages nuget or other package?

    public class EmployerIncentiveClaimSubmittedEvent : Event
    {
        public Guid IncentiveClaimApprenticeshipId { get; set; }
    }
}
