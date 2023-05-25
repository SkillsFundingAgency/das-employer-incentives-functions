using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals
{
    public interface IWithdrawalService
    {
        Task Withdraw(WithdrawRequest request);
        Task Reinstate(ReinstateApplicationRequest request);
    }
}
