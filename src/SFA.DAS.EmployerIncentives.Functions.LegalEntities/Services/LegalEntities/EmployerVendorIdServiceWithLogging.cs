using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class EmployerVendorIdServiceWithLogging : IEmployerVendorIdService
    {
        private readonly IEmployerVendorIdService _addEmployerVendorIdService;
        private readonly ILogger<EmployerVendorIdServiceWithLogging> _logger;

        public EmployerVendorIdServiceWithLogging(
            IEmployerVendorIdService addEmployerVendorIdService,
            ILogger<EmployerVendorIdServiceWithLogging> logger)
        {
            _addEmployerVendorIdService = addEmployerVendorIdService;
            _logger = logger;
        }

        public async Task GetAndAddEmployerVendorId(string hashedLegalEntityId)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"Calling IAddEmployerVendorIdService.GetAndAddEmployerVendorId with parameters: [hashedLegalEntityId={hashedLegalEntityId}]");

                await _addEmployerVendorIdService.GetAndAddEmployerVendorId(hashedLegalEntityId);

                _logger.Log(LogLevel.Information, $"Called IAddEmployerVendorIdService.GetAndAddEmployerVendorId with parameters: [hashedLegalEntityId={hashedLegalEntityId}]");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling IAddEmployerVendorIdService.GetAndAddEmployerVendorId with parameters: [hashedLegalEntityId={hashedLegalEntityId}]");

                throw;
            }
        }
    }
}
