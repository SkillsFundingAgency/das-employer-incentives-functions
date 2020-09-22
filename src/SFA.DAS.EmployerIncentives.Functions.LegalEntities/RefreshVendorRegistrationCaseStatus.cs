using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class RefreshVendorRegistrationCaseStatus
    {
        private readonly IVendorRegistrationFormService _vendorRegistrationFormService;
        private readonly IVrfCaseRefreshConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RefreshVendorRegistrationCaseStatus(IVendorRegistrationFormService vendorRegistrationFormService,
            IVrfCaseRefreshConfiguration configuration,
            IDateTimeProvider dateTimeProvider)
        {
            _vendorRegistrationFormService = vendorRegistrationFormService;
            _configuration = configuration;
            _dateTimeProvider = dateTimeProvider;
        }

        // Every hour: "* 0 * * * *"
        [FunctionName("RefreshVendorRegistrationCaseStatus")]
        public async Task Run([TimerTrigger("%RefreshVendorRegistrationCaseStatusTriggerTime%")] TimerInfo myTimer, ILogger log)
        {
            var from = await _configuration.GetLastRunDateTime();
            var to = await _dateTimeProvider.GetCurrentDateTime();

            log.Log(LogLevel.Information, $"Calling RefreshVendorRegistrationFormStatuses with parameters: [dateTimeFrom={from}] & [dateTimeTo={to}]");

            await _vendorRegistrationFormService.RefreshVendorRegistrationFormStatuses(from, to);

            await _configuration.UpdateLastRunDateTime(to);
        }
    }
}
