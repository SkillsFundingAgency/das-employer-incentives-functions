using System;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawls.Types
{
    public class WithdrawRequest
    {
        public WithdrawlType Type { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long ULN { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
    }
}
