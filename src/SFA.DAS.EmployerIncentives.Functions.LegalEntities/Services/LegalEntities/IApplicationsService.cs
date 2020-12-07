using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public interface IApplicationsService
    {
        Task<IEnumerable<Application>> GetApplications(long accountId);
    }
}
