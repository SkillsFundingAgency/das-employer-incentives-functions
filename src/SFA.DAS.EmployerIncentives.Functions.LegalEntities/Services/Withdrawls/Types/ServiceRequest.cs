using System;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawls.Types
{
    public class ServiceRequest
    {
        public string TaskId { get; set; }
        public string DecisionReference { get; set; }
        public DateTime? TaskCreatedDate { get; set; }
    }
}
