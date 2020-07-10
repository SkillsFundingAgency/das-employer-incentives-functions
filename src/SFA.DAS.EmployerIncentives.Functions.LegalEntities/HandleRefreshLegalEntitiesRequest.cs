using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleRefreshLegalEntitiesRequest
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public HandleRefreshLegalEntitiesRequest(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        [FunctionName("HttpTriggerRefreshLegalEntities")]
        public Task RunHttp([HttpTrigger(AuthorizationLevel.Function)] HttpRequest request)
        {
            return _employerIncentivesService.RefreshLegalEntities();
        }
    }
}
