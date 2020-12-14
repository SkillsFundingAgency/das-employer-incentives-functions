
namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types
{
    public class CollectionCalendarUpdateRequest
    {
        public short CollectionPeriodYear { get; }
        public byte CollectionPeriodNumber { get; }
        public bool Active { get; }

        public CollectionCalendarUpdateRequest(short calendarYear, byte periodNumber, bool active)
        {
            CollectionPeriodYear = calendarYear;
            CollectionPeriodNumber = periodNumber;
            Active = active;
        }
    }
}
