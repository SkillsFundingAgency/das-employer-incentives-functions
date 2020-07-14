using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives.Types;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.EmployerIncentives.Messages.Events;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleRefreshLegalEntityEvent
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public HandleRefreshLegalEntityEvent(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        [FunctionName("HandleRefreshLegalEntityEvent")]
        public Task RunEvent([NServiceBusTrigger(Endpoint = QueueNames.RefreshLegalEntity)] RefreshLegalEntityEvent message)
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
