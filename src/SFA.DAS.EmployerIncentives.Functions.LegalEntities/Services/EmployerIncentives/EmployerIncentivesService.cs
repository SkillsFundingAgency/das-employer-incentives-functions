using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives.Types;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives
{
    public class EmployerIncentivesService : IEmployerIncentivesService
    {
        private readonly HttpClient _client;

        public EmployerIncentivesService(HttpClient client)
        {
            _client = client;
        }
        
        public Task RefreshLegalEntities()
        {
            return RefreshLegalEntities(1, 200);
        }

        public async Task RefreshLegalEntities(int pageNumber, int pageSize)
        {
            var data = new Dictionary<string, object>
            {
                { "PageNumber", pageNumber },
                { "PageSize", pageSize }
            };

            var response = await _client.PutAsJsonAsync(
                "/jobs", 
                new JobRequest { Type = JobType.RefreshLegalEntities, Data = data 
                });

            response.EnsureSuccessStatusCode();

        }

        public async Task AddLegalEntity(AddLegalEntityRequest request)
        {
            var response = await _client.PostAsJsonAsync($"/accounts/{request.AccountId}/legalEntities", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveLegalEntity(RemoveLegalEntityRequest request)
        {
            var response = await _client.DeleteAsync($"/accounts/{request.AccountId}/legalEntities/{request.AccountLegalEntityId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
