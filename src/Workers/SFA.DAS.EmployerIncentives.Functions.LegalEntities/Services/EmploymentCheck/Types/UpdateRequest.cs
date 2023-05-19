using System;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types
{
    public class UpdateRequest
    {
        public Guid CorrelationId { get; set; }
        public string Result { get; set; }
        public DateTime DateChecked { get; set; }
    }
}
