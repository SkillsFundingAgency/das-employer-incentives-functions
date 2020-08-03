using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class LegalEntitiesService : ILegalEntitiesService
    {
        private readonly HttpClient _client;
        private readonly IJobsService _jobsService;        

        public LegalEntitiesService(HttpClient client, IJobsService jobsService)
        {
            _client = client;
            _jobsService = jobsService;
        }
        
        public Task Refresh()
        {
            return Refresh(1, 200);
        }

        public Task Refresh(int pageNumber, int pageSize)
        {
            return _jobsService.RefreshLegalEntities(pageNumber, pageSize);
        }

        public async Task Add(AddRequest request)
        {
            var response = await _client.PostAsJsonAsync($"accounts/{request.AccountId}/legalEntities", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task Remove(RemoveRequest request)
        {
            var response = await _client.DeleteAsync($"accounts/{request.AccountId}/legalEntities/{request.AccountLegalEntityId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
