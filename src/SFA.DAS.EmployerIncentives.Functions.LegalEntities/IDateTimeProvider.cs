using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public interface IDateTimeProvider
    {
        Task<DateTime> GetCurrentDateTime();
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public Task<DateTime> GetCurrentDateTime()
        {
            return Task.FromResult(DateTime.UtcNow);
        }
    }
}