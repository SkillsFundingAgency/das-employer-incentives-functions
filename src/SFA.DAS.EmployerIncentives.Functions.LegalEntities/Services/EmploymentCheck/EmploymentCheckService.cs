using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck
{
    public class EmploymentCheckService : IEmploymentCheckService
    {
        private readonly HttpClient _client;

        public EmploymentCheckService(HttpClient client)
        {
            _client = client;
        }

        public async Task Update(UpdateRequest request)
        {
            var response = await _client.PutAsJsonAsync($"employmentchecks/{request.CorrelationId}", request);
            response.EnsureSuccessStatusCode();
        }
    }
}
