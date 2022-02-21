using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments.Types;

#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments
{
    public class PausePaymentsService : IPausePaymentsService
    {
        private readonly HttpClient _client;

        public PausePaymentsService(HttpClient client)
        {
            _client = client;
        }

        public async Task SetPauseStatus(PausePaymentsRequest request)
        {
            var response = await _client.PostAsJsonAsync("pause-payments", request);

            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new PausePaymentServiceException(response.StatusCode, await GetContentAsString(response));
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
