using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.LegalEntities
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
    }
}
