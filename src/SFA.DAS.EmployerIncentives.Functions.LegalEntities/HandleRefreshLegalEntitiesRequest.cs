using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function)] HttpRequest request)
        {
            await _employerIncentivesService.RefreshLegalEntities();

            return new OkResult();
        }
    }
}
