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

        public async Task Add(EmployerVendorId employerVendorId)
        {
            var url = $"legalentities/{employerVendorId.HashedLegalEntityId}/employervendorid";

            var response = await _client.PutAsync(url, new StringContent(""));
            response.EnsureSuccessStatusCode();
        }
    }
}