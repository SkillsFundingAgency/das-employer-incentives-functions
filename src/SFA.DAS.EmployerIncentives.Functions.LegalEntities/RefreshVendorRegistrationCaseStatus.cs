using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class RefreshVendorRegistrationCaseStatus
    {
        private readonly IVendorRegistrationFormService _vendorRegistrationFormService;

        public RefreshVendorRegistrationCaseStatus(IVendorRegistrationFormService vendorRegistrationFormService)
        {
            _vendorRegistrationFormService = vendorRegistrationFormService;
        }

        [FunctionName("RefreshVendorRegistrationCaseStatus")]
        public async Task Run([TimerTrigger("%RefreshVendorRegistrationCaseStatusTriggerTime%", RunOnStartup = false)] TimerInfo myTimer)
        {
            await _vendorRegistrationFormService.RefreshVendorRegistrationFormStatuses();
        }
    }
}
