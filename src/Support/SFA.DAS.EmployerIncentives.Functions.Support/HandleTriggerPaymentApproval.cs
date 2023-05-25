﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs;

namespace SFA.DAS.EmployerIncentives.Functions.Support
{
    public class HandleTriggerPaymentApproval
    {
        private readonly IJobsService _jobsService;

        public HandleTriggerPaymentApproval(IJobsService jobsService)
        {
            _jobsService = jobsService;
        }

        [FunctionName("HttpTriggerPaymentApproval")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function)] HttpRequest request)
        {
            await _jobsService.TriggerPaymentApproval();

            return new OkResult();
        }
    }
}
