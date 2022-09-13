using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using SFA.DAS.EmployerIncentives.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Types;

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
                    Content = ErrorContent(ex)
                };
            }
            catch (EmploymentCheckServiceException ex)
            {
                return new ContentResult()
                {
                    StatusCode = (int)ex.HttpStatusCode,
                    ContentType = "application/json",
                    Content = ex.Content
                };
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

        private static string ErrorContent(Exception ex)
        {
            string paramName = (ex is ArgumentException argEx) ? argEx.ParamName : "";
            string message = (ex is EmploymentCheckServiceException ecEx) ? ecEx.Content : ex.Message;

            return JsonConvert.SerializeObject(new
            {
                paramName,
                message,
                Example = new EmploymentCheckRequest
                {
                    CheckType = "InitialEmploymentChecks|EmployedAt365DaysCheck",
                    Applications = new List<Application>
                    {
                        new Application { AccountLegalEntityId = 1234, ULN = 567890 },
                        new Application { AccountLegalEntityId = 2345, ULN = 678901 }
                    }.ToArray(),
                    ServiceRequest = new ServiceRequest()
                    {
                        TaskId = "taskId1234_optional",
                        DecisionReference = "decisionReference123_optional",
                        TaskCreatedDate = DateTime.UtcNow
                    }
                }
            });
        }
    }
}
