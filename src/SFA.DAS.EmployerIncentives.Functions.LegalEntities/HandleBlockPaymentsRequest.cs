using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments.Types;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleBlockPaymentsRequest
    {
        private readonly IBlockPaymentsService _blockPaymentsService;

        public HandleBlockPaymentsRequest(IBlockPaymentsService blockPaymentsService)
        {
            _blockPaymentsService = blockPaymentsService;
        }

        [FunctionName("HttpTriggerHandleBlockPaymentsRequest")]
        public async Task<IActionResult> RunHttp(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "block-payments")]
            HttpRequestMessage request)
        {
            try
            {
                var blockPaymentsRequest =
                    JsonConvert.DeserializeObject<List<BlockAccountLegalEntityForPaymentsRequest>>(
                        await request.Content.ReadAsStringAsync());
                await _blockPaymentsService.BlockAccountLegalEntitiesForPayments(blockPaymentsRequest);
            }
            catch (ArgumentException ex)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(new
                    {
                        ex.ParamName,
                        ex.Message,
                        Example = new
                        {
                            VendorBlocks = new List<VendorBlock>
                            {
                                new VendorBlock
                                    { VendorId = "P10001234", VendorBlockEndDate = new DateTime(2022, 01, 01) },
                                new VendorBlock
                                    { VendorId = "P10001255", VendorBlockEndDate = new DateTime(2022, 02, 01) }
                            },
                            ServiceRequest = new ServiceRequest
                            {
                                TaskId = "taskId1234",
                                DecisionReference = "decisionReference123",
                                TaskCreatedDate = DateTime.UtcNow
                            }
                        }
                    })
                };
            }
            catch (BlockPaymentsServiceException ex)
            {
                return new ContentResult
                {
                    StatusCode = (int)ex.HttpStatusCode,
                    ContentType = "application/json",
                    Content = ex.Content
                };
            }
            catch (Exception ex)
            {
                return new ContentResult
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