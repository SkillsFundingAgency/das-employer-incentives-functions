using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Infrastructure.Extensions;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class VendorRegistrationFormService : IVendorRegistrationFormService
    {
        private readonly HttpClient _client;
        private readonly IVrfCaseRefreshRepository _repository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger _logger;

        public VendorRegistrationFormService(HttpClient client,
            IVrfCaseRefreshRepository repository,
            IDateTimeProvider dateTimeProvider,
            ILogger logger)
        {
            _client = client;
            _repository = repository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task RefreshVendorRegistrationFormStatuses()
        {
            var from = await _repository.GetLastRunDateTime();
            var to = await _dateTimeProvider.GetCurrentDateTime();

            _logger.Log(LogLevel.Information, $"Calling RefreshVendorRegistrationFormStatuses with parameters: [dateTimeFrom={from}] & [dateTimeTo={to}]");

            var url = $"legalentities/vendorregistrationform/status?from={from.ToIsoDateTime()}&to={to.ToIsoDateTime()}";

            var response = await _client.PatchAsync(url, new StringContent(""));
            response.EnsureSuccessStatusCode();

            await _repository.UpdateLastRunDateTime(to);
        }
    }
}
