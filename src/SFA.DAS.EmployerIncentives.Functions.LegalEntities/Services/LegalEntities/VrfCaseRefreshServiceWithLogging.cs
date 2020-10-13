using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class VrfCaseRefreshServiceWithLogging : IVrfCaseRefreshService
    {
        private readonly IVrfCaseRefreshService _vrfCaseRefreshService;
        private readonly ILogger<VrfCaseRefreshServiceWithLogging> _logger;

        public VrfCaseRefreshServiceWithLogging(
            IVrfCaseRefreshService vrfCaseRefreshService,
            ILogger<VrfCaseRefreshServiceWithLogging> logger)
        {
            _vrfCaseRefreshService = vrfCaseRefreshService;
            _logger = logger;
        }

        public async Task RefreshStatuses()
        {
            try
            {
                _logger.Log(LogLevel.Information, $"Calling IVrfCaseRefreshService.RefreshStatuses");

                await _vrfCaseRefreshService.RefreshStatuses();

                _logger.Log(LogLevel.Information, $"Called IVrfCaseRefreshService.RefreshStatuses");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling IVrfCaseRefreshService.RefreshStatuses");

                throw;
            }
        }
    }
}
