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

        public async Task Pause()
        {
            try
            {
                _logger.LogInformation("[VRF Refresh] Calling IVrfCaseRefreshService.Pause");

                await _vrfCaseRefreshService.Pause();

                _logger.LogInformation("[VRF Refresh] Called IVrfCaseRefreshService.Pause");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[VRF Refresh] Error calling IVrfCaseRefreshService.Pause");

                throw;
            }
        }

        public async Task Refresh()
        {
            try
            {
                _logger.LogInformation("[VRF Refresh] Calling IVrfCaseRefreshService.Refresh");

                await _vrfCaseRefreshService.Refresh();

                _logger.LogInformation("[VRF Refresh] Called IVrfCaseRefreshService.Refresh");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[VRF Refresh] Error calling IVrfCaseRefreshService.Refresh");

                throw;
            }
        }

        public async Task Resume()
        {
            try
            {
                _logger.LogInformation("[VRF Refresh] Calling IVrfCaseRefreshService.Resume");

                await _vrfCaseRefreshService.Resume();

                _logger.LogInformation("[VRF Refresh] Called IVrfCaseRefreshService.Resume");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[VRF Refresh] Error calling IVrfCaseRefreshService.Resume");

                throw;
            }
        }
    }
}
