using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives.Types;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleRemoveLegalEntityEvent
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public HandleRemoveLegalEntityEvent(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        [FunctionName("HandleRemovedLegalEntityEvent")]
        public Task RunEvent([NServiceBusTrigger(Endpoint = QueueNames.RemovedLegalEntity)] RemovedLegalEntityEvent message)
        {
            var removeRequest = new RemoveLegalEntityRequest
            {
                AccountId = message.AccountId,
                AccountLegalEntityId = message.AccountLegalEntityId
            };

            return _employerIncentivesService.RemoveLegalEntity(removeRequest);
        }
    }
}
