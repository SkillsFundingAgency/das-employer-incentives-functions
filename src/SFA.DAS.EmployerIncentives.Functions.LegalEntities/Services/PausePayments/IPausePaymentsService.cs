using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments.Types;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments
{
    public interface IPausePaymentsService
    {
        Task SetPauseStatus(PausePaymentsRequest request);
    }
}
