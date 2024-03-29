﻿using System.Collections.Generic;
using System.Net;
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

        public async Task Refresh(IEnumerable<EmploymentCheckRequest> requests)
        {
            var response = await _client.PutAsJsonAsync(
               $"jobs",
               new JobRequest
               {
                   Type = JobType.RefreshEmploymentChecks,
                   Data = new Dictionary<string, string>
                    {
                        { "Requests", JsonConvert.SerializeObject(requests) }
                    }
               });

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new EmploymentCheckServiceException(response.StatusCode, await GetContentAsString(response));
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task Update(UpdateRequest request)
        {
            var response = await _client.PutAsJsonAsync($"employmentchecks/{request.CorrelationId}", request);
            response.EnsureSuccessStatusCode();
        }
        
        public async Task<string> GetContentAsString(HttpResponseMessage response)
        {
            string content = null;
            try
            {
                content = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                // Do nothing
            }
            return content;
        }
    }
}
