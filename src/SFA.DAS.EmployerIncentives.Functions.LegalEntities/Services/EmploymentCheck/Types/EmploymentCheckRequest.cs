using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Types;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types
{
    public class EmploymentCheckRequest
    {
        public string CheckType { get; set; }
        public Application[] Applications { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
    }
    public enum RefreshEmploymentCheckType
    {
        InitialEmploymentChecks,
        EmployedAt365DaysCheck
    }
}
