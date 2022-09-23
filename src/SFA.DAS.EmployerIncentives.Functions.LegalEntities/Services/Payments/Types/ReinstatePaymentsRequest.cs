using System;
using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments.Types
{
    public class ReinstatePaymentsRequest
    {
        public List<Guid> Payments { get; set; }
        public ReinstatePaymentsServiceRequest ServiceRequest { get; set; }
    }

    public class ReinstatePaymentsServiceRequest : ServiceRequest
    {
        public string Process { get; set; }
    }
}
