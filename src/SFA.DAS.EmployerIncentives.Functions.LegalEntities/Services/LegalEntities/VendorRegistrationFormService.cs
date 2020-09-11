using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.HashingService;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class VendorRegistrationFormService : IVendorRegistrationFormService
    {
        private readonly HttpClient _client;
        private readonly IJobsService _jobsService;
        private readonly IHashingService _hashingService;

        public VendorRegistrationFormService(HttpClient client, IJobsService jobsService, IHashingService hashingService)
        {
            _client = client;
            _jobsService = jobsService;
            _hashingService = hashingService;
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

        public async Task UpdateVrfCaseStatus(long legalEntityId, string caseId)
        {
            var response = await _client.PatchAsync($"legalentities/{legalEntityId}/vendorregistrationform/{caseId}", new StringContent(""));
            response.EnsureSuccessStatusCode();
        }
    }
}
