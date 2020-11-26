using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class EarningsResilienceCheckService : IEarningsResilienceCheckService
    {
        private readonly HttpClient _client;

        public EarningsResilienceCheckService(HttpClient client)
        {
            _client = client;
        }
        public async Task Update()
        {
            var url = "earnings-resilience-check";

            var response = await _client.PostAsync(url, new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }
    }
}
