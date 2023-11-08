using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class RefreshVendorRegistrationCaseStatus
    {
        private readonly IVrfCaseRefreshService _vrfCaseRefreshService;

        public RefreshVendorRegistrationCaseStatus(IVrfCaseRefreshService vrfCaseRefreshService)
        {
            _vrfCaseRefreshService = vrfCaseRefreshService;
        }

        [FunctionName("RefreshVendorRegistrationCaseStatus")]
        public async Task Run([TimerTrigger("%RefreshVendorRegistrationCaseStatusTriggerTime%", RunOnStartup = false)] TimerInfo myTimer)
        {
            await _vrfCaseRefreshService.Refresh();
        }

        [FunctionName("HttpTriggerPauseRefreshVendorRegistrationCaseStatus")]
        public async Task<IActionResult> Pause([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "pause-vrf-case-refresh")] HttpRequestMessage request)
        {
            try
            {
                await _vrfCaseRefreshService.Pause();
            }           
            catch (Exception ex)
            {
                return new ContentResult()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(new { ex.Message })
                };
            }

            return new OkResult();
        }

        [FunctionName("HttpTriggerResumeRefreshVendorRegistrationCaseStatus")]
        public async Task<IActionResult> Resume([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "resume-vrf-case-refresh")] HttpRequestMessage request)
        {
            try
            {
                await _vrfCaseRefreshService.Resume();
            }
            catch (Exception ex)
            {
                return new ContentResult()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(new { ex.Message })
                };
            }

            return new OkResult();
        }

    }
}
