using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments.Types;

#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments
{
    public class PaymentsService : IPaymentsService
    {
        private readonly HttpClient _client;

        public PaymentsService(HttpClient client)
        {
            _client = client;
        }

        public async Task SetPauseStatus(PausePaymentsRequest request)
        {
            var response = await _client.PostAsJsonAsync("pause-payments", request);

            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new PaymentsServiceException(response.StatusCode, await GetContentAsString(response));
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task RevertPayments(RevertPaymentsRequest request)
        {
            var response = await _client.PostAsJsonAsync("revert-payments", request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new PaymentsServiceException(response.StatusCode, await GetContentAsString(response));
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
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
