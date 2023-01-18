using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs
{
    public interface IJobsService
    {
        Task RefreshLegalEntities(int pageNumber, int pageSize);
    }
}
