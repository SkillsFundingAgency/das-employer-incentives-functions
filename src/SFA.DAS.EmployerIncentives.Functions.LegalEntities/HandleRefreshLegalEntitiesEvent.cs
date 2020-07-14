using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.EmployerIncentives.Messages.Events;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleRefreshLegalEntitiesEvent
    {
        private readonly ILegalEntitiesService _legalEntitiesService;
                public HandleRefreshLegalEntitiesEvent(ILegalEntitiesService legalEntitiesService)
        {
            _legalEntitiesService = legalEntitiesService;
        }

        [FunctionName("HandleRefreshLegalEntitiesEvent")]
        public Task RunEvent([NServiceBusTrigger(Endpoint = QueueNames.RefreshLegalEntities)] RefreshLegalEntitiesEvent message)
        {
            return _legalEntitiesService.Refresh(message.PageNumber, message.PageSize);
        }
    }
}
