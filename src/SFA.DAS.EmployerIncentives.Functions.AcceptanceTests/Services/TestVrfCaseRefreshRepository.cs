using System;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services
{
    public class TestVrfCaseRefreshRepository : IVrfCaseRefreshRepository
    {
        private DateTime _lastRunDateTime;

        public TestVrfCaseRefreshRepository()
        {
            _lastRunDateTime = DateTime.Now;
        }

        public Task<DateTime> GetLastRunDateTime()
        {
            return Task.FromResult(_lastRunDateTime);
        }

        public Task UpdateLastRunDateTime(DateTime value)
        {
            _lastRunDateTime = value;

            return Task.CompletedTask;
        }
    }
}
