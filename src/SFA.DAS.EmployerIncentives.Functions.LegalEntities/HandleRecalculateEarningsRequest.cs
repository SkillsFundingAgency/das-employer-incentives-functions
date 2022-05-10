using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RecalculateEarnings;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RecalculateEarnings.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleRecalculateEarningsRequest
    {
        private readonly IRecalculateEarningsService _recalculateEarningsService;

        public HandleRecalculateEarningsRequest(IRecalculateEarningsService recalculateEarningsService)
        {
            _recalculateEarningsService = recalculateEarningsService;
        }

        [FunctionName("HttpTriggerRecalculateEarningsRequest")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "recalculate-earnings")] HttpRequestMessage request)
        {
            try
            {
                var recalculateEarningsRequest =
                    JsonConvert.DeserializeObject<RecalculateEarningsRequest>(await request.Content.ReadAsStringAsync());
                await _recalculateEarningsService.RecalculateEarnings(recalculateEarningsRequest);
            }
            catch (ArgumentException ex)
            {
                return new ContentResult()
                {
                    StatusCode = (int) HttpStatusCode.BadRequest,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(new
                    {
                        ex.ParamName,
                        ex.Message,
                        Example = new
                        {
                            IncentiveLearnerIdentifiers = new List<IncentiveLearnerIdentifierDto>
                            {
                                new IncentiveLearnerIdentifierDto { AccountLegalEntityId = 1234, ULN = 11112222 },
                                new IncentiveLearnerIdentifierDto { AccountLegalEntityId = 2222, ULN = 33331111 },
                                new IncentiveLearnerIdentifierDto { AccountLegalEntityId = 3333, ULN = 22221111 }
                            }
                        }
                    })
                };
            }
            catch (RecalculateEarningsServiceException ex)
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
