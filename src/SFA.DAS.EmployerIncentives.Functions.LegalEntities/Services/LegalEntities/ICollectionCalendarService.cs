
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public interface ICollectionCalendarService
    {
        Task UpdatePeriod(CollectionCalendarUpdateRequest updateRequest);
    }
}
