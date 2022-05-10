using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments.Types;
using SFA.DAS.EmployerIncentives.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandlePausePaymentsRequest
    {
        private readonly IPausePaymentsService _pausePaymentsService;

        public HandlePausePaymentsRequest(IPausePaymentsService pausePaymentsService)
        {
            _pausePaymentsService = pausePaymentsService;
        }

        [FunctionName("HttpTriggerPausePaymentsRequest")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "pause-payments")] HttpRequestMessage request)
        {
            try
            {
                var pausePaymentsRequest =
                    JsonConvert.DeserializeObject<PausePaymentsRequest>(await request.Content.ReadAsStringAsync());
                await _pausePaymentsService.SetPauseStatus(pausePaymentsRequest);
            }
            catch (ArgumentException ex)
            {
                return new ContentResult()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ContentType = "application/json",
                    Content = ErrorContent(ex)
                };
            }
            catch (PausePaymentServiceException ex)
            {
                return new ContentResult()
                {
                    StatusCode = (int)ex.HttpStatusCode,
                    ContentType = "application/json",
                    Content = ErrorContent(ex)
                };

            }
            catch (Exception ex)
            {
                return new ContentResult()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ContentType = "application/json",
                    Content = ErrorContent(ex)
                };
            }

            return new OkResult();
        }

        private static string ErrorContent(Exception ex)
        {
            string paramName = (ex is ArgumentException argEx) ? argEx.ParamName : "";

            return JsonConvert.SerializeObject(new
            {
                ParamName = paramName,
                ex.Message,
                Example = new
                {
                    Action = "Pause|Resume",
                    Applications = new List<object>()
                    {
                        new
                        {
                            AccountLegalEntityId = 1234,
                            ULN = 5678
                        },
                        new
                        {
                            AccountLegalEntityId = 5678,
                            ULN = 9876
                        }
                    },
                    ServiceRequest = new ServiceRequest()
                    {
                        TaskId = "taskId1234",
                        DecisionReference = "decisionReference123",
                        TaskCreatedDate = DateTime.UtcNow
                    }
                }
            });
        }
    }
}
