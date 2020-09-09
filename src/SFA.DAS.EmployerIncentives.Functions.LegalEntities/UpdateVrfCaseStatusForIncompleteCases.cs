using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class UpdateVrfCaseStatusForIncompleteCases
    {
        private readonly IVendorRegistrationFormService _vendorRegistrationFormService;

        public UpdateVrfCaseStatusForIncompleteCases(IVendorRegistrationFormService vendorRegistrationFormService)
        {
            _vendorRegistrationFormService = vendorRegistrationFormService;
        }

        [FunctionName("UpdateVrfCaseStatusForIncompleteCases")]
        public async Task Run([TimerTrigger("0 0 1 * * *")]TimerInfo myTimer, ILogger log)
        {
            await _vendorRegistrationFormService.UpdateVrfCaseStatus();
        }
    }
}
