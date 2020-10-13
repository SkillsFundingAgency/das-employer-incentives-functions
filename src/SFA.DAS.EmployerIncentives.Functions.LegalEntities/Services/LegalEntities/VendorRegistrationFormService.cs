using SFA.DAS.EmployerIncentives.Infrastructure.Extensions;
using System;
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
        }

        public async Task Update(DateTime fromDateTime)
        {
            var url = $"legalentities/vendorregistrationform/status?from={fromDateTime.ToIsoDateTime()}";

            var response = await _client.PatchAsync(url, new StringContent(""));
            response.EnsureSuccessStatusCode();
        }
    }
}
