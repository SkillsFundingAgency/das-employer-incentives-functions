using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types
{
    public class EmploymentCheckRequest
    {
        public long AccountLegalEntityId { get; set; }
        public long ULN { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
    }
}
