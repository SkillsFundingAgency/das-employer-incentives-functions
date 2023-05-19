using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs.Types
{
    public class JobRequest
    {
        public JobType Type { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}
