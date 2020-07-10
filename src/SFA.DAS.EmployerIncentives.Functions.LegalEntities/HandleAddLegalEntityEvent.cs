using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives.Types;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleAddLegalEntityEvent
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public HandleAddLegalEntityEvent(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        [FunctionName("HandleAddedLegalEntityEvent")]
        public Task RunEvent([NServiceBusTrigger(Endpoint = QueueNames.LegalEntityAdded)] AddedLegalEntityEvent message)
        {
            var addRequest = new AddLegalEntityRequest
            {
                AccountId = message.AccountId,
                AccountLegalEntityId = message.AccountLegalEntityId,
                LegalEntityId = message.LegalEntityId,
                OrganisationName = message.OrganisationName
            };
            
            return _employerIncentivesService.AddLegalEntity(addRequest);
        }
    }
}
