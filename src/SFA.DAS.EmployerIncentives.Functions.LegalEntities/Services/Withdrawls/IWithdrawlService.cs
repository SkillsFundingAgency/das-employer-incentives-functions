using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawls.Types;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawls
{
    public interface IWithdrawlService
    {
        Task Withdraw(WithdrawRequest request);
    }
}
