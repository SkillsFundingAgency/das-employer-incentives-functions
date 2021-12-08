using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using SFA.DAS.EmployerIncentives.Types;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleRefreshEmploymentCheckRequest
    {
        private readonly IEmploymentCheckService _employmentCheckService;

        public HandleRefreshEmploymentCheckRequest(IEmploymentCheckService employmentCheckService)
        {
            _employmentCheckService = employmentCheckService;
        }

        [FunctionName("HttpTriggerEmploymentCheckRequest")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "employmentchecks")] HttpRequestMessage request)
        {
            try
            {
                var employmentCheckRequest = JsonConvert.DeserializeObject<EmploymentCheckRequest>(await request.Content.ReadAsStringAsync());
                await _employmentCheckService.Refresh(employmentCheckRequest);
            }
            catch(ArgumentException ex)
            {
                return new ContentResult()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(new { 
                        ex.ParamName, 
                        ex.Message, 
                        Example = new  
                        {
                            AccountLegalEntityId = 1234,
                            ULN = 5678,
                            ServiceRequest = new ServiceRequest()
                            {
                                 TaskId = "taskId1234_optional",
                                 DecisionReference = "decisionReference123_optional",
                                 TaskCreatedDate = DateTime.UtcNow
                            }
                        }
                    })
                };
            }
            catch(Exception ex)
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
