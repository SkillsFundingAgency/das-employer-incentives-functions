using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives.Types
{
    public class JobRequest
    {
        public JobType Type { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
