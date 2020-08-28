using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class TestTimerFunction
    {
        private readonly ILegalEntitiesService _legalEntitiesService;

        public TestTimerFunction(ILegalEntitiesService legalEntitiesService)
        {
            _legalEntitiesService = legalEntitiesService;
        }

        [FunctionName("TestTimerFunction")]
        public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = false)] TimerInfo myTimer)
        {
            var addRequest = new AddRequest
            {
                AccountId = 1,
                AccountLegalEntityId = 2,
                LegalEntityId = 3,
                OrganisationName = "OrganisationName"
            };

            await _legalEntitiesService.Add(addRequest);
        }
    }
}
