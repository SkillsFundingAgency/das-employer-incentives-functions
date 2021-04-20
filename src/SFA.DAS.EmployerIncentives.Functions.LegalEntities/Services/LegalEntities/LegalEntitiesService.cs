using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class LegalEntitiesService : ILegalEntitiesService
    {
        private readonly HttpClient _client;

        public LegalEntitiesService(HttpClient client)
        {
            _client = client;
        }
        
        public Task Refresh()
        {
            return Refresh(1, 200);
        }

        public async Task Refresh(int pageNumber, int pageSize)
        {
            var url = $"legalentities/refresh?pageNumber={pageNumber}&pageSize={pageSize}";
            var response = await _client.PutAsync(url, new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task Add(AddRequest request)
        {
            var response = await _client.PutAsJsonAsync($"accounts/{request.AccountId}/legalEntities", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task Remove(RemoveRequest request)
        {
            var response = await _client.DeleteAsync($"accounts/{request.AccountId}/legalEntities/{request.AccountLegalEntityId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
