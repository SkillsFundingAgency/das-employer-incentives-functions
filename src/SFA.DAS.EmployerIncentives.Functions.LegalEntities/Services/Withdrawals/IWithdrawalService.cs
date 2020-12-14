using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals.Types;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals
{
    public interface IWithdrawalService
    {
        Task Withdraw(WithdrawRequest request);
    }
}
