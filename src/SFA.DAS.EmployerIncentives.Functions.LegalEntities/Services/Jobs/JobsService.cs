using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs.Types;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs
{
    public class JobsService : IJobsService
    {
        private readonly HttpClient _client;

        public JobsService(HttpClient client)
        {
            _client = client;
        }

        public async Task RefreshLegalEntities(int pageNumber, int pageSize)
        {
            var data = new Dictionary<string, string>
            {
                { "PageNumber", pageNumber.ToString() },
                { "PageSize", pageSize.ToString() }
            };

            var response = await _client.PutAsJsonAsync(
                $"jobs", 
                new JobRequest { Type = JobType.RefreshLegalEntities, Data = data 
                });

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateVrfCaseDetailsForNewApplications()
        {
            var response = await _client.PutAsJsonAsync(
                $"jobs",
                new JobRequest
                {
                    Type = JobType.UpdateVrfCaseDetailsForNewApplications,
                    Data = null
                });

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateVrfCaseStatusForIncompleteCases()
        {
            var response = await _client.PutAsJsonAsync(
                $"jobs",
                new JobRequest
                {
                    Type = JobType.UpdateVrfCaseStatusForIncompleteCases,
                    Data = null
                });

            response.EnsureSuccessStatusCode();
        }
    }
}
