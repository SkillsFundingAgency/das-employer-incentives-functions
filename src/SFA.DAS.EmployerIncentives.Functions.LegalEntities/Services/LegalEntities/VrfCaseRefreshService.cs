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

        public async Task RefreshStatuses()
        {
            var from = await _repository.GetLastRunDateTime();

            _logger.LogInformation("[VRF Refresh] Updating vrf case status from {from}", from);

            var lastCaseUpdate = await _vrfService.Update(from);

            _logger.LogInformation("[VRF Refresh] Updating vrf case status last run date time to {lastCaseUpdate}", lastCaseUpdate);

            await _repository.UpdateLastRunDateTime(lastCaseUpdate);
        }
    }
}
