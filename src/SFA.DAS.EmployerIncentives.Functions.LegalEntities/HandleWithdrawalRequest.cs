using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals.Types;
using SFA.DAS.EmployerIncentives.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleWithdrawalRequest
    {
        private readonly IWithdrawalService _withdrawalService;

        public HandleWithdrawalRequest(IWithdrawalService withdrawalService)
        {
            _withdrawalService = withdrawalService;
        }

        [FunctionName("HttpTriggerWithdrawalRequest")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "withdraw")] HttpRequestMessage request)
        {
            try
            {
                var withdrawRequest = JsonConvert.DeserializeObject<WithdrawRequest>(await request.Content.ReadAsStringAsync());
                await _withdrawalService.Withdraw(withdrawRequest);
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
                            WithdrawalType = "Employer|Compliance",
                            Applications = new List<Application>
                            {
                                new Application { AccountLegalEntityId = 123, ULN = 345 },
                                new Application { AccountLegalEntityId = 243, ULN = 4567 }
                            },
                            ServiceRequest = new ServiceRequest()
                            {
                                 TaskId = "taskId1234",
                                 DecisionReference = "decisionReference123",
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
