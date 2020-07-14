using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleRemoveLegalEntityEvent
    {
        private readonly ILegalEntitiesService _legalEntitiesService;

        public HandleRemoveLegalEntityEvent(ILegalEntitiesService legalEntitiesService)
        {
            _legalEntitiesService = legalEntitiesService;
        }

        [FunctionName("HandleRemovedLegalEntityEvent")]
        public Task RunEvent([NServiceBusTrigger(Endpoint = QueueNames.RemovedLegalEntity)] RemovedLegalEntityEvent message)
        {
            var removeRequest = new RemoveRequest
            {
                AccountId = message.AccountId,
                AccountLegalEntityId = message.AccountLegalEntityId
            };

            return _legalEntitiesService.Remove(removeRequest);
        }
    }
}
