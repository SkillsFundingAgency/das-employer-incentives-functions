using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class RefreshVendorRegistrationCaseStatus
    {
        private readonly IVendorRegistrationFormService _vrfCaseRefreshService;

        public RefreshVendorRegistrationCaseStatus(IVendorRegistrationFormService vrfCaseRefreshService)
        {
            _vrfCaseRefreshService = vrfCaseRefreshService;
        }

        [FunctionName("RefreshVendorRegistrationCaseStatus")]
        public async Task Run([TimerTrigger("%RefreshVendorRegistrationCaseStatusTriggerTime%", RunOnStartup = true)] TimerInfo myTimer)
        {
            await _vrfCaseRefreshService.Refresh();
        }
    }
}
