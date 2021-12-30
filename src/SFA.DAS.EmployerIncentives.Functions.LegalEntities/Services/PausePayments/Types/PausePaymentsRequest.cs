using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments.Types
{
    public class PausePaymentsRequest
    {
        public PausePaymentsAction Action { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long ULN { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
    }
}
