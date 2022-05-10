using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals.Types;
using SFA.DAS.EmployerIncentives.Types;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
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
                        Example = new[] {
                            new {
                                AccountLegalEntityId = 1234,
                                ULN = 5678
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
