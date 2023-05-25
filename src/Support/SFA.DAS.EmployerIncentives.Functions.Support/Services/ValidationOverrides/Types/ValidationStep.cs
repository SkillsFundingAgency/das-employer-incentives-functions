using System;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.ValidationOverrides.Types
{
    public class ValidationStep
    {
        public ValidationType ValidationType { get; set; }
        public bool? Remove { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
