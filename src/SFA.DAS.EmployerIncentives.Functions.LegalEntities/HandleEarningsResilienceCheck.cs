using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleEarningsResilienceCheck
    {
        private readonly IEarningsResilienceCheckService _earningsResilienceCheckService;

        public HandleEarningsResilienceCheck(IEarningsResilienceCheckService earningsResilienceCheckService)
        {
            _earningsResilienceCheckService = earningsResilienceCheckService;
        }

        [FunctionName("TimerEarningsResilienceCheck")]
        public async Task RunTimer([TimerTrigger("%EarningsResilienceCheckTriggerTime%", RunOnStartup = false)]TimerInfo myTimer, ILogger log)
        {
            await _earningsResilienceCheckService.Update();
        }

        [FunctionName("HttpEarningsResilienceCheck")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function)] HttpRequest request)
        {
            await _earningsResilienceCheckService.Update();

            return new OkResult();
        }
    }
}
