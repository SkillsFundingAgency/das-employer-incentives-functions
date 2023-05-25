using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs
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
                "jobs", 
                new JobRequest { Type = JobType.RefreshLegalEntities, Data = data 
                });

            response.EnsureSuccessStatusCode();
        }

        public async Task RefreshLearnerMatch()
        {
            var response = await _client.PutAsJsonAsync(
                "jobs",
                new JobRequest { Type = JobType.RefreshLearnerMatch, Data = new Dictionary<string, string>()
                });

            response.EnsureSuccessStatusCode();
        }

        public async Task TriggerPaymentValidation()
        {
            var response = await _client.PutAsJsonAsync(
                "jobs",
                new JobRequest
                {
                    Type = JobType.PaymentValidation,
                    Data = new Dictionary<string, string>()
                });

            response.EnsureSuccessStatusCode();
        }

        public async Task TriggerPaymentApproval()
        {
            var response = await _client.PutAsJsonAsync(
                "jobs",
                new JobRequest
                {
                    Type = JobType.PaymentApproval,
                    Data = new Dictionary<string, string>()
                });

            response.EnsureSuccessStatusCode();
        }
    }
}
