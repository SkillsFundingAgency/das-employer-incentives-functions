using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals.Types;
using System;
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
                            Type = "Employer|Compliance",
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
