using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Types;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments.Types
{
    public class PausePaymentsRequest
    {
        public PausePaymentsAction Action { get; set; }
        public Application[] Applications { get; set; }

        public ServiceRequest ServiceRequest { get; set; }
    }
}
