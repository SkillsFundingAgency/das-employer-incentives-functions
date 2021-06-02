using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public interface IVrfCaseRefreshService
    {
        Task Refresh();
        Task Pause();
        Task Resume();
    }
}
