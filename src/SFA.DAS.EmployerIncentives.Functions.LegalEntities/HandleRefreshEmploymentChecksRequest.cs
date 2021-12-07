using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleRefreshEmploymentChecksRequest
    {
        private readonly IJobsService _jobsService;

        public HandleRefreshEmploymentChecksRequest(IJobsService jobsService)
        {
            _jobsService = jobsService;
        }

        [FunctionName("HttpTriggerRefreshEmploymentChecks")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function, "POST")] HttpRequestMessage request)
        {
            await _jobsService.RefreshEmploymentChecks();

            return new OkResult();
        }
    }
}
