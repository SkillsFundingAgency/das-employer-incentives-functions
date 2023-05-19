using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.BlockPayments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.BlockPayments
{
    public interface IBlockPaymentsService
    {
        Task BlockAccountLegalEntitiesForPayments(BlockAccountLegalEntityForPaymentsRequest blockPaymentsRequest);
    }
}
