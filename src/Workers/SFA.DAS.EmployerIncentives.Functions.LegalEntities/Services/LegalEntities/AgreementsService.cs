using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class AgreementsService : IAgreementsService
    {
        private readonly HttpClient _client;

        public AgreementsService(HttpClient client)
        {
            _client = client;
        }
        
        public async Task SignAgreement(SignAgreementRequest request)
        {
            var response = await _client.PatchAsync($"accounts/{request.AccountId}/legalEntities/{request.AccountLegalEntityId}", request.GetStringContent());
            response.EnsureSuccessStatusCode();
        }
    }
}
