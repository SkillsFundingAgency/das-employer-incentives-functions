using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RecalculateEarnings.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RecalculateEarnings
{
    public interface IRecalculateEarningsService
    {
        Task RecalculateEarnings(RecalculateEarningsRequest recalculateEarningsRequest);
    }
}
