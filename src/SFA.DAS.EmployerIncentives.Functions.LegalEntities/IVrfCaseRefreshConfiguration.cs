using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public interface IVrfCaseRefreshConfiguration
    {
        Task<DateTime> GetLastRunDateTime();
        Task UpdateLastRunDateTime(DateTime value);
    }
}