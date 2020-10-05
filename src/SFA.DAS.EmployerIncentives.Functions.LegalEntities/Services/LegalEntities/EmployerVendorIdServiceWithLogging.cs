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

        public async Task AddEmployerVendorId(string hashedLegalEntityId)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"Calling IAddEmployerVendorIdService.AddEmployerVendorId with parameters: [hashedLegalEntityId={hashedLegalEntityId}]");

                await _addEmployerVendorIdService.AddEmployerVendorId(hashedLegalEntityId);

                _logger.Log(LogLevel.Information, $"Called IAddEmployerVendorIdService.AddEmployerVendorId with parameters: [hashedLegalEntityId={hashedLegalEntityId}]");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling IAddEmployerVendorIdService.AddEmployerVendorId with parameters: [hashedLegalEntityId={hashedLegalEntityId}]");

                throw;
            }
        }
    }
}
