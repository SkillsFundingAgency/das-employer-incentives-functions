using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ValidationOverrides.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ValidationOverrides
{
    public interface IValidationOverrideService
    {
        Task Add(IEnumerable<ValidationOverride> requests);
    }
}
