using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives.Types;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives
{
    public interface IEmployerIncentivesService
    {
        Task RefreshLegalEntities();
        Task RefreshLegalEntities(int pageNumber, int pageSize);
        Task AddLegalEntity(AddLegalEntityRequest request);
        Task RemoveLegalEntity(RemoveLegalEntityRequest request);            
    }
}
