using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs;

namespace SFA.DAS.EmployerIncentives.Functions.Support
{
    public class HandleRefreshLearnerMatchRequest
    {
        private readonly IJobsService _jobsService;

        public HandleRefreshLearnerMatchRequest(IJobsService jobsService)
        {
            _jobsService = jobsService;
        }

        [FunctionName("HttpTriggerRefreshLearnerMatch")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function)] HttpRequest request)
        {
            await _jobsService.RefreshLearnerMatch();

            return new OkResult();
        }
    }
}
