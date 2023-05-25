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

        public async Task<DateTime> GetLastRunDateTime()
        {
            return _lastRunDateTime;
        }

        public async Task UpdateLastRunDateTime(DateTime value)
        {
            _lastRunDateTime = value;
        }
    }
}
