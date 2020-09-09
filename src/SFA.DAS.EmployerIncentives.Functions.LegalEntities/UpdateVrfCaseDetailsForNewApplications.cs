using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class UpdateVrfCaseDetailsForNewApplications
    {
        private readonly IVendorRegistrationFormService _vendorRegistrationFormService;

        public UpdateVrfCaseDetailsForNewApplications(IVendorRegistrationFormService vendorRegistrationFormService)
        {
            _vendorRegistrationFormService = vendorRegistrationFormService;
        }

        [FunctionName("UpdateVrfCaseDetailsForNewApplications")]
        public async Task Run([TimerTrigger("0 1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            await _vendorRegistrationFormService.UpdateVrfCaseDetails();
        }
    }
}
