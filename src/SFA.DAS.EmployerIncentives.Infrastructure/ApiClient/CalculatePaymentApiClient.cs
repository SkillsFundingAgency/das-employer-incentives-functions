using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Infrastructure.ApiClient
{
    public class CalculatePaymentApiClient : ApiClientBase<CalculatePaymentApiClient>, ICalculatePaymentApiClient
    {
        public CalculatePaymentApiClient(HttpClient httpClient, ILogger<CalculatePaymentApiClient> logger)
        : base(httpClient, logger)
        {
        }

        public Task CalculateFirstPayment(Guid claimId)
        {
            // TODO: integrate with outer API
            return Task.CompletedTask;
        }
    }
}
