using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments.Types
{
    public class BlockAccountLegalEntityForPaymentsRequest
    {
        public List<VendorBlock> VendorBlocks { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
    }
}
