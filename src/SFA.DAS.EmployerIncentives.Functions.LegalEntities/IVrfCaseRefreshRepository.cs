using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public interface IVrfCaseRefreshRepository
    {
        Task<VrfCaseRefresh> Get();
        Task Update(VrfCaseRefresh vrfCaseRefresh);
    }
}