using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs.Types;
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

        public async Task Refresh(EmploymentCheckRequest request)
        {
            var response = await _client.PutAsJsonAsync(
               $"jobs",
               new JobRequest
               {
                   Type = JobType.RefreshEmploymentCheck,
                   Data = new System.Collections.Generic.Dictionary<string, string>
                    {
                        { "AccountLegalEntityId", request.AccountLegalEntityId.ToString() },
                        { "ULN", request.ULN.ToString() },
                        { "ServiceRequest", JsonConvert.SerializeObject(request.ServiceRequest) }
                    }
               });

            response.EnsureSuccessStatusCode();
        }

        public async Task Update(UpdateRequest request)
        {
            var response = await _client.PutAsJsonAsync($"employmentchecks/{request.CorrelationId}", request);
            response.EnsureSuccessStatusCode();
        }
    }
}
