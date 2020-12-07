using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types
{
    public class Application
    {
        public long AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public string LegalEntityName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal TotalIncentiveAmount { get; set; }
        public string Status { get; set; }
    }
}
