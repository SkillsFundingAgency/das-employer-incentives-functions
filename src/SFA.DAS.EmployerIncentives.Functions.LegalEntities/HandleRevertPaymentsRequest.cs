using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments.Types;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleRevertPaymentsRequest
    {
        private readonly IPaymentsService _paymentsService;

        public HandleRevertPaymentsRequest(IPaymentsService paymentsService)
        {
            _paymentsService = paymentsService;
        }

        [FunctionName("HttpTriggerRevertPaymentsRequest")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "revert-payments")] HttpRequestMessage request)
        {
            try
            {
                var recalculateEarningsRequest =
                    JsonConvert.DeserializeObject<RevertPaymentsRequest>(await request.Content.ReadAsStringAsync());
                await _paymentsService.RevertPayments(recalculateEarningsRequest);
            }
            catch (ArgumentException ex)
            {
                return new ContentResult()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(new
                    {
                        ex.ParamName,
                        ex.Message,
                        Example = new
                        {
                            Payments = new List<string> { "c4db8994d04f4689abaca82da5686c6e" },
                            ServiceRequest = new ServiceRequest
                            {
                                TaskId = "TASK1234",
                                DecisionReference = "DEC123",
                                TaskCreatedDate = new DateTime(2022, 07, 01, 12, 34, 56)
                            }
                        }
                    })
                };
            }
            catch (PaymentsServiceException ex)
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
    }
}
