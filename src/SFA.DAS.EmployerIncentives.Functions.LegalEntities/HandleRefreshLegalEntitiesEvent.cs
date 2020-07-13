using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System.Threading.Tasks;


namespace SFA.DAS.EmployerIncentives
{
    public class RefreshLegalEntitiesEvent // TODO: update nuget
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleRefreshLegalEntitiesEvent
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public HandleRefreshLegalEntitiesEvent(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        [FunctionName("HandleRefreshLegalEntitiesEvent")]
        public Task RunEvent([NServiceBusTrigger(Endpoint = QueueNames.RefreshLegalEntities)] RefreshLegalEntitiesEvent message)
        {
            return _employerIncentivesService.RefreshLegalEntities(message.PageNumber, message.PageSize);
        }
    }
}
