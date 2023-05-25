using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals.Types;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support
{
    public class HandleReinstateApplicationRequest
    {
        private readonly IWithdrawalService _withdrawalService;

        public HandleReinstateApplicationRequest(IWithdrawalService withdrawalService)
        {
            _withdrawalService = withdrawalService;
        }

        [FunctionName("HttpTriggerReinstateApplicationRequest")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "reinstate")] HttpRequestMessage request)
        {
            try
            {
                var reinstateApplicationRequest = JsonConvert.DeserializeObject<ReinstateApplicationRequest>(await request.Content.ReadAsStringAsync());
                await _withdrawalService.Reinstate(reinstateApplicationRequest);
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
                        Example = new ReinstateApplicationRequest
                        {
                            Applications = new List<Application>
                            {
                                new Application
                                {
                                    AccountLegalEntityId = 1234,
                                            ULN = 5678,
                                            ServiceRequest = new ServiceRequest
                                            {
                                                TaskId = "taskId1234",
                                                DecisionReference = "decisionReference123",
                                                TaskCreatedDate = DateTime.UtcNow
                                            }
                                },
                                new Application
                                {
                                    AccountLegalEntityId = 2345,
                                    ULN = 6789,
                                    ServiceRequest = new ServiceRequest
                                    {
                                        TaskId = "taskId1234",
                                        DecisionReference = "decisionReference123",
                                        TaskCreatedDate = DateTime.UtcNow
                                    }
                                }
                            }.ToArray()
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
