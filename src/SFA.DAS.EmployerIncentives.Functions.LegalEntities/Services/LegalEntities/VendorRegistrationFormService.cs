using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using SFA.DAS.EmployerIncentives.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class VendorRegistrationFormService : IVendorRegistrationFormService
    {
        private readonly HttpClient _client;

        public VendorRegistrationFormService(HttpClient client)
        {
            _client = client;
            _client.Timeout = TimeSpan.FromMinutes(5);
        }

        public async Task<DateTime> Update(DateTime fromDateTime)
        {
            var url = $"legalentities/vendorregistrationform/status?from={fromDateTime.ToIsoDateTime()}";

            var response = await _client.PatchAsync(url, new StringContent(""));
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();

            var lastCaseUpdatedDateTime = JsonConvert.DeserializeObject<DateTime>(data);

            return lastCaseUpdatedDateTime;
        }
    }
}
