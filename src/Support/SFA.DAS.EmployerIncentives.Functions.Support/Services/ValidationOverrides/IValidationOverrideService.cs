using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.ValidationOverrides.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.ValidationOverrides
{
    public interface IValidationOverrideService
    {
        Task Add(IEnumerable<ValidationOverride> requests);
    }
}
