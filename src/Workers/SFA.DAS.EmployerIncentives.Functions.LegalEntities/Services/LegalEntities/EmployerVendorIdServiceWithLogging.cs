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

        public async Task Add(EmployerVendorId employerVendorId)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"Calling IAddEmployerVendorIdService.GetAndAddEmployerVendorId with parameters: [hashedLegalEntityId={employerVendorId?.HashedLegalEntityId}]");

                await _addEmployerVendorIdService.Add(employerVendorId);

                _logger.Log(LogLevel.Information, $"Called IAddEmployerVendorIdService.GetAndAddEmployerVendorId with parameters: [hashedLegalEntityId={employerVendorId?.HashedLegalEntityId}]");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling IAddEmployerVendorIdService.GetAndAddEmployerVendorId with parameters: [hashedLegalEntityId={employerVendorId?.HashedLegalEntityId}]");

                throw;
            }
        }
    }
}
