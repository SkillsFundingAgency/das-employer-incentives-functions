using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RevertPayments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RevertPayments
{
    public interface IRevertPaymentsService
    {
        Task RevertPayments(RevertPaymentsRequest request);
    }
}
