using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class EarningsResilienceCheckServiceWithLogging : IEarningsResilienceCheckService
    {
        private readonly IEarningsResilienceCheckService _earningsResilienceCheckService;
        private readonly ILogger<EmployerVendorIdServiceWithLogging> _logger;

        public EarningsResilienceCheckServiceWithLogging(
            IEarningsResilienceCheckService earningsResilienceCheckService,
            ILogger<EmployerVendorIdServiceWithLogging> logger)
        {
            _earningsResilienceCheckService = earningsResilienceCheckService;
            _logger = logger;
        }

        public async Task Update()
        {
            try
            {
                _logger.Log(LogLevel.Information, $"Calling IEarningsResilienceCheckService.Update");

                await _earningsResilienceCheckService.Update();

                _logger.Log(LogLevel.Information, $"Called IEarningsResilienceCheckService.Update");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling IEarningsResilienceCheckService.Update");

                throw;
            }
        }
    }
}
