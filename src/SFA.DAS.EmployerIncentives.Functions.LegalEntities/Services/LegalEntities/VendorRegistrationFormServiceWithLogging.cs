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

        public async Task Refresh()
        {
            try
            {
                _logger.LogInformation("[VRF Refresh] Calling VendorRegistrationFormService.Refresh");

                await _vendorRegistrationFormService.Refresh();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[VRF Refresh] Error calling VendorRegistrationFormService.Refresh");

                throw;
            }
        }
    }
}
