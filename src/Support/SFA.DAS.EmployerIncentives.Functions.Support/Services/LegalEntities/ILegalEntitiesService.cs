using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.LegalEntities
{
    public interface ILegalEntitiesService
    {
        Task Refresh();
        Task Refresh(int pageNumber, int pageSize);
    }
}
