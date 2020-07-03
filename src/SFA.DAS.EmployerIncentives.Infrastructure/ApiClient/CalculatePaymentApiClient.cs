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

        public async Task CalculateFirstPayment(long accountId, Guid claimId)
        {
            _logger.LogInformation($"Submitting calculate first payment request for account {accountId} claim {claimId}");

            await PostAsJson($"/account/{accountId}/claim/{claimId}", false);
        }
    }
}
