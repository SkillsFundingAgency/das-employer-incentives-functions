using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class VendorRegistrationFormServiceWithLogging : IVendorRegistrationFormService
    {
        private readonly IVendorRegistrationFormService _vendorRegistrationFormService;
        private readonly ILogger<VendorRegistrationFormServiceWithLogging> _logger;

        public VendorRegistrationFormServiceWithLogging(
            IVendorRegistrationFormService vendorRegistrationFormService,
            ILogger<VendorRegistrationFormServiceWithLogging> logger)
        {
            _vendorRegistrationFormService = vendorRegistrationFormService;
            _logger = logger;
        }


        public async Task Update(DateTime fromDateTime, DateTime toDateTime)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"Calling IVendorRegistrationFormService.Update with parameters: [dateTimeFrom={fromDateTime}] & [dateTimeTo={toDateTime}]");

                await _vendorRegistrationFormService.Update(fromDateTime, toDateTime);

                _logger.Log(LogLevel.Information, $"Called IVendorRegistrationFormService.Update with parameters: [dateTimeFrom={fromDateTime}] & [dateTimeTo={toDateTime}]");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling IVendorRegistrationFormService.Update with parameters: [dateTimeFrom={fromDateTime}] & [dateTimeTo={toDateTime}]");

                throw;
            }
        }
    }
}
