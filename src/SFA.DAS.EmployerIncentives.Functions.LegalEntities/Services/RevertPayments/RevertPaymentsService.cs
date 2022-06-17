using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RevertPayments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RevertPayments
{
    public class RevertPaymentsService : IRevertPaymentsService
    {
        private readonly HttpClient _client;

        public RevertPaymentsService(HttpClient client)
        {
            _client = client;
        }

        public async Task RevertPayments(RevertPaymentsRequest request)
        {
            var response = await _client.PostAsJsonAsync("revert-payments", request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new RevertPaymentsServiceException(response.StatusCode, await GetContentAsString(response));
            }

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
