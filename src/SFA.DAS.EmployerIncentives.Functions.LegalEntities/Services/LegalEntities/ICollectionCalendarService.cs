
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public interface ICollectionCalendarService
    {
        Task ActivatePeriod(short calendarYear, byte periodNumber);
    }
}
