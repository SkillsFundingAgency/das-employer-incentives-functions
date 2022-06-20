using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments
{
    public interface IPaymentsService
    {
        Task SetPauseStatus(PausePaymentsRequest request);
        Task RevertPayments(RevertPaymentsRequest request);
    }
}
