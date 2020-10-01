using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class VrfCaseRefreshService : IVrfCaseRefreshService
    {
        private readonly IVendorRegistrationFormService _vrfService;
        private readonly IVrfCaseRefreshRepository _repository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public VrfCaseRefreshService(
            IVendorRegistrationFormService vrfService,
            IVrfCaseRefreshRepository repository,
            IDateTimeProvider dateTimeProvider)
        {
            _vrfService = vrfService;
            _repository = repository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task RefreshStatuses()
        {
            var from = await _repository.GetLastRunDateTime();
            var to = await _dateTimeProvider.GetCurrentDateTime();

            await _vrfService.Update(from, to);

            await _repository.UpdateLastRunDateTime(to);
        }
    }
}
