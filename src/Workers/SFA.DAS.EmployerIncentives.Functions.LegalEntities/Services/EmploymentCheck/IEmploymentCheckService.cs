using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck
{
    public interface IEmploymentCheckService
    {
        Task Update(UpdateRequest request);

        Task Refresh(IEnumerable<EmploymentCheckRequest> requests);
    }
}
