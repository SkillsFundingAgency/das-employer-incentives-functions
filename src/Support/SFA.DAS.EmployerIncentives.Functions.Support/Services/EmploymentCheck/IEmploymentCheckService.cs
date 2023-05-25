using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.EmploymentCheck.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.EmploymentCheck
{
    public interface IEmploymentCheckService
    {
        Task Update(UpdateRequest request);

        Task Refresh(IEnumerable<EmploymentCheckRequest> requests);
    }
}
