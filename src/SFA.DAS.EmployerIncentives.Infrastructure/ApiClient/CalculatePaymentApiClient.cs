using Microsoft.Extensions.Logging;
using SFA.DAS.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Infrastructure.ApiClient
{
    public class CalculatePaymentApiClient : RestHttpClient, ICalculatePaymentApiClient
    {
        private readonly ILogger<CalculatePaymentApiClient> _logger;

        public CalculatePaymentApiClient(HttpClient httpClient, ILogger<CalculatePaymentApiClient> logger)
        :base(httpClient)
        {
            _logger = logger;
        }

        public async Task<bool> CalculateFirstPayment(long accountId, Guid claimId)
        {
            _logger.LogInformation($"Submitting calculate payment request for account {accountId} claim {claimId}");

            try
            {
                await PostAsJson($"/account/{accountId}/claim/{claimId}", false);
                return await Task.FromResult(true);
            }
            catch (RestHttpClientException ex)
            {
                _logger.LogError(ex, $"Http error code returned when submitting calculate payment request for account {accountId} claim {claimId}");
                return await Task.FromResult(false);
            }
        }
    }
}
