
namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types
{
    public class CollectionCalendarUpdateRequest
    {
        public short AcademicYear { get; }
        public byte PeriodNumber { get; }
        public bool Active { get; }

        public CollectionCalendarUpdateRequest(short academicYear, byte periodNumber, bool active)
        {
            AcademicYear = academicYear;
            PeriodNumber = periodNumber;
            Active = active;
        }
    }
}
