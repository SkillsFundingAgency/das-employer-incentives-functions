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
            _client.Timeout = TimeSpan.FromMinutes(5);
        }

        public async Task Refresh()
        {
            const string url = "legalentities/vendorregistrationform";

            var response = await _client.PatchAsync(url, new StringContent(""));
            response.EnsureSuccessStatusCode();
        }
    }
}
