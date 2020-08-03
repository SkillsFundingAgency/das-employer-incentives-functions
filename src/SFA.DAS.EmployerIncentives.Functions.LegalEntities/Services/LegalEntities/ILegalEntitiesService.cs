using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public interface ILegalEntitiesService
    {
        Task Refresh();
        Task Refresh(int pageNumber, int pageSize);
        Task Add(AddRequest request);
        Task Remove(RemoveRequest request);
        Task SignAgreement(SignAgreementRequest request);
    }
}
