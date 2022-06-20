using System;
using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments.Types
{
    public class RevertPaymentsRequest
    {
        public List<Guid> Payments { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
    }
}
