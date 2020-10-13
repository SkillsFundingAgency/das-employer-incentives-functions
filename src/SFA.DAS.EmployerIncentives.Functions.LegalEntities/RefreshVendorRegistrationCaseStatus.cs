using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class RefreshVendorRegistrationCaseStatus
    {
        private readonly IVrfCaseRefreshService _vrfCaseRefreshService;

        public RefreshVendorRegistrationCaseStatus(IVrfCaseRefreshService vrfCaseRefreshService)
        {
            _vrfCaseRefreshService = vrfCaseRefreshService;
        }

        [FunctionName("RefreshVendorRegistrationCaseStatus")]
        public async Task Run([TimerTrigger("%RefreshVendorRegistrationCaseStatusTriggerTime%", RunOnStartup = false)] TimerInfo myTimer)
        {
            await _vrfCaseRefreshService.RefreshStatuses();
        }
    }
}
