
namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types
{
    public class CollectionCalendarUpdateRequest
    {
        public string AcademicYear { get; }
        public byte PeriodNumber { get; }
        public bool Active { get; }

        public CollectionCalendarUpdateRequest(string academicYear, byte periodNumber, bool active)
        {
            AcademicYear = academicYear;
            PeriodNumber = periodNumber;
            Active = active;
        }
    }
}
