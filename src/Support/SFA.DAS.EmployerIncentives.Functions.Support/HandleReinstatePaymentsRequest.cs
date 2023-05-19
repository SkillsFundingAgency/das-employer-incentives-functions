using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Payments;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Payments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support
{
    public class HandleReinstatePaymentsRequest
    {
        private readonly IPaymentsService _paymentsService;

        public HandleReinstatePaymentsRequest(IPaymentsService paymentsService)
        {
            _paymentsService = paymentsService;
        }

        [FunctionName("HttpTriggerReinstatePaymentsRequest")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "reinstate-payments")] HttpRequestMessage request)
        {
            try
            {
                var reinstatePaymentsRequest =
                    JsonConvert.DeserializeObject<ReinstatePaymentsRequest>(await request.Content.ReadAsStringAsync());
                await _paymentsService.ReinstatePayments(reinstatePaymentsRequest);
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
                            Payments = new List<Guid> { Guid.NewGuid() },
                            ServiceRequest = new ReinstatePaymentsServiceRequest()
                            {
                                TaskId = "TASK1234",
                                DecisionReference = "DEC123",
                                TaskCreatedDate = new DateTime(2022, 07, 01, 12, 34, 56),
                                Process = "ManualPayments"
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
