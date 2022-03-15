using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ValidationOverrides.Types;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ValidationOverrides
{
    public class ValidationOverrideService : IValidationOverrideService
    {
        private readonly HttpClient _client;

        public ValidationOverrideService(HttpClient client)
        {
            _client = client;
        }

        public async Task Add(IEnumerable<ValidationOverride> requests)
        {  
            var response = await _client.PostAsJsonAsync("validation-overrides", new ValidationOverrideRequest { ValidationOverrides =  requests.ToArray() });

            response.EnsureSuccessStatusCode();
        }
    }
}
