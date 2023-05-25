using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.RecalculateEarnings.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.RecalculateEarnings
{
    public interface IRecalculateEarningsService
    {
        Task RecalculateEarnings(RecalculateEarningsRequest recalculateEarningsRequest);
    }
}
