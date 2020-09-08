using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class UpdateVrfCaseDetailsForNewApplications
    {
        private readonly ILegalEntitiesService _legalEntitiesService;

        public UpdateVrfCaseDetailsForNewApplications(ILegalEntitiesService legalEntitiesService)
        {
            _legalEntitiesService = legalEntitiesService;
        }

        [FunctionName("UpdateVrfCaseDetailsForNewApplications")]
        public async Task Run([TimerTrigger("0 1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            await _legalEntitiesService.UpdateVrfCaseDetails();
        }
    }
}
