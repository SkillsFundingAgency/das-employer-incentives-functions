using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.LegalEntities;

namespace SFA.DAS.EmployerIncentives.Functions.Support
{
    public class HandleRefreshLegalEntitiesRequest
    {
        private readonly ILegalEntitiesService _legalEntitiesService;

        public HandleRefreshLegalEntitiesRequest(ILegalEntitiesService legalEntitiesService)
        {
            _legalEntitiesService = legalEntitiesService;
        }

        [FunctionName("HttpTriggerRefreshLegalEntities")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function)] HttpRequest request)
        {
            await _legalEntitiesService.Refresh();

            return new OkResult();
        }
    }
}
