using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class VrfCaseRefreshService : IVrfCaseRefreshService
    {
        private readonly IVendorRegistrationFormService _vrfService;
        private readonly IVrfCaseRefreshRepository _repository;

        public VrfCaseRefreshService(
            IVendorRegistrationFormService vrfService,
            IVrfCaseRefreshRepository repository)
        {
            _vrfService = vrfService;
            _repository = repository;
        }

        public async Task RefreshStatuses()
        {
            var from = await _repository.GetLastRunDateTime();

            var lastCaseUpdate = await _vrfService.Update(from);

            await _repository.UpdateLastRunDateTime(lastCaseUpdate);
        }
    }
}
