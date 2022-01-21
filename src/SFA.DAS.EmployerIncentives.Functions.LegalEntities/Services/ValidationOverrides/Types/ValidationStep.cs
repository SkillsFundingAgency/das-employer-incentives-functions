using System;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ValidationOverrides.Types
{
    public class ValidationStep
    {
        public ValidationType ValidationType { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
