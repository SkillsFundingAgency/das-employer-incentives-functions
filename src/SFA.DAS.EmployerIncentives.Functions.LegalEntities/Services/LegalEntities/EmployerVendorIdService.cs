using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class EmployerVendorIdService : IEmployerVendorIdService
    {
        private readonly HttpClient _client;

        public EmployerVendorIdService(HttpClient client)
        {
            _client = client;
        }

        public async Task AddEmployerVendorId(string hashedLegalEntityId)
        {
            var url = $"legalentities/{hashedLegalEntityId}/employervendorid";

            var response = await _client.PostAsync(url, new StringContent(""));
            response.EnsureSuccessStatusCode();
        }
    }
}