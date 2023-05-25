using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleAddLegalEntityEvent
    {
        private readonly ILegalEntitiesService _legalEntitiesService;

        public HandleAddLegalEntityEvent(ILegalEntitiesService legalEntitiesService)
        {
            _legalEntitiesService = legalEntitiesService;
        }

        [FunctionName("HandleAddedLegalEntityEvent")]
        public Task RunEvent([NServiceBusTrigger(Endpoint = QueueNames.LegalEntityAdded)] AddedLegalEntityEvent message)
        {
            var addRequest = new AddRequest
            {
                AccountId = message.AccountId,
                AccountLegalEntityId = message.AccountLegalEntityId,
                LegalEntityId = message.LegalEntityId,
                OrganisationName = message.OrganisationName
            };
            
            return _legalEntitiesService.Add(addRequest);
        }
    }
}
