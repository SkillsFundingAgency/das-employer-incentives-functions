using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs.Types
{
    public class JobRequest
    {
        public JobType Type { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}
