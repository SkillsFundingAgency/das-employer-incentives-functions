using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class VrfCaseRefreshService : IVrfCaseRefreshService
    {
        private readonly IVendorRegistrationFormService _vrfService;
        private readonly IVrfCaseRefreshRepository _repository;
        private readonly ILogger<VrfCaseRefreshService> _logger;

        public VrfCaseRefreshService(
            IVendorRegistrationFormService vrfService,
            IVrfCaseRefreshRepository repository,
            ILogger<VrfCaseRefreshService> logger)
        {
            _vrfService = vrfService;
            _repository = repository;
            _logger = logger;
        }

        public async Task Pause()
        {
            var vrfCaseRefreshStatus = await _repository.Get();
            vrfCaseRefreshStatus.IsPaused = true;
            await _repository.Update(vrfCaseRefreshStatus);
        }

        public async Task Refresh()
        {
            var vrfCaseRefreshStatus = await _repository.Get();
            if (!vrfCaseRefreshStatus.IsPaused)
            {
                _logger.LogInformation("[VRF Refresh] Updating vrf case status from {from}", vrfCaseRefreshStatus.LastRunDateTime);

                var lastCaseUpdate = await _vrfService.Update(vrfCaseRefreshStatus.LastRunDateTime);

                _logger.LogInformation("[VRF Refresh] Updating vrf case status last run date time to {lastCaseUpdate}", lastCaseUpdate);

                vrfCaseRefreshStatus.LastRunDateTime = lastCaseUpdate;
                await _repository.Update(vrfCaseRefreshStatus);
            }
        }

        public async Task Resume()
        {
            var vrfCaseRefreshStatus = await _repository.Get();
            vrfCaseRefreshStatus.IsPaused = false;
            await _repository.Update(vrfCaseRefreshStatus);
        }
    }
}
