using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments
{
    public interface IBlockPaymentsService
    {
        Task BlockAccountLegalEntitiesForPayments(List<BlockAccountLegalEntityForPaymentsRequest> blockPaymentsRequest);
    }
}