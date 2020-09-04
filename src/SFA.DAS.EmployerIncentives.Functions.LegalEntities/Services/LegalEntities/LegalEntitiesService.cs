using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.HashingService;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class LegalEntitiesService : ILegalEntitiesService
    {
        private readonly HttpClient _client;
        private readonly IJobsService _jobsService;
        private readonly IHashingService _hashingService;

        public LegalEntitiesService(HttpClient client, IJobsService jobsService, IHashingService hashingService)
        {
            _client = client;
            _jobsService = jobsService;
            _hashingService = hashingService;
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

        public Task UpdateVrfCaseDetails()
        {
            return _jobsService.UpdateVrfCaseDetailsForNewApplications();
        }

        public async Task UpdateVrfCaseDetails(long legalEntityId)
        {
            var hashedLegalEntityId = _hashingService.HashValue(legalEntityId);

            var response = await _client.PatchAsync($"legalentities/{legalEntityId}/vendorregistrationform?hashedLegalEntityId={hashedLegalEntityId}", new StringContent(""));
            response.EnsureSuccessStatusCode();
        }

        public Task UpdateVrfCaseStatus()
        {
            return _jobsService.UpdateVrfCaseStatusForIncompleteCases();
        }
    }
}
