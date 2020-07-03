using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerIncentives.Infrastructure.Configuration;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerIncentives.Infrastructure.ApiClient
{
    public class CalculatePaymentsApiClientFactory : ICalculatePaymentsApiClientFactory
    {
        private readonly ClientApiConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public CalculatePaymentsApiClientFactory(IOptions<ClientApiConfiguration> configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration.Value;
            _loggerFactory = loggerFactory;
        }

        public ICalculatePaymentApiClient CreateClient()
        {
            var httpClientFactory = new AzureActiveDirectoryHttpClientFactory(_configuration, _loggerFactory);
            var httpClient = httpClientFactory.CreateHttpClient();
            var apiClient = new CalculatePaymentApiClient(httpClient, _loggerFactory.CreateLogger<CalculatePaymentApiClient>());

            return apiClient;
        }
    }
}
