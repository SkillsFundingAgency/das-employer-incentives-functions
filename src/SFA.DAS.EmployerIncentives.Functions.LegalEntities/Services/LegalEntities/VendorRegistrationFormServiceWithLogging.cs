using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Infrastructure.Extensions;
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

        public async Task Update(DateTime fromDateTime)
        {
            try
            {
                _logger.LogInformation($"Calling IVendorRegistrationFormService.Update with parameters: [dateTimeFrom={fromDateTime.ToIsoDateTime()}]", fromDateTime);

                await _vendorRegistrationFormService.Update(fromDateTime);

                _logger.LogInformation($"Called IVendorRegistrationFormService.Update with parameters: [dateTimeFrom={fromDateTime.ToIsoDateTime()}]", fromDateTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling IVendorRegistrationFormService.Update with parameters: [dateTimeFrom={fromDateTime.ToIsoDateTime()}]", fromDateTime);

                throw;
            }
        }
    }
}
