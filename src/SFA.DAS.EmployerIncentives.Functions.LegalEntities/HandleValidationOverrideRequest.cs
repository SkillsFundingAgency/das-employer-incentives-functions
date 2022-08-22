using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ValidationOverrides;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ValidationOverrides.Types;
using SFA.DAS.EmployerIncentives.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleValidationOverrideRequest
    {
        private readonly IValidationOverrideService _validationOverrideService;

        public HandleValidationOverrideRequest(IValidationOverrideService validationOverrideService)
        {
            _validationOverrideService = validationOverrideService;
        }

        [FunctionName("HttpTriggerValidationOverrideRequest")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "validation-override")] HttpRequestMessage request)
        {
            try
            {
                var overrideRequests = JsonConvert.DeserializeObject<ValidationOverride[]>(await request.Content.ReadAsStringAsync());
                await _validationOverrideService.Add(overrideRequests);
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
            string paramName = (ex is ArgumentException argEx) ? argEx.ParamName: "";

            return JsonConvert.SerializeObject(new
            {
                ParamName = paramName,
                ex.Message,
                Example = new List<object>()
                        {
                            new
                            {
                                AccountLegalEntityId = 1234,
                                ULN = 5678,
                                ValidationSteps = new List<object>()
                                {
                                    new
                                    {
                                        ValidationType = "HasDaysInLearning|IsInLearning|HasNoDataLocks|EmployedBeforeSchemeStarted|EmployedAtStartOfApprenticeship|EmployedAt365Days",
                                        ExpiryDate = DateTime.UtcNow.AddDays(10).Date,
                                        Remove = "true|false|(optional)"
                                    },
                                    new
                                    {
                                        ValidationType = "HasDaysInLearning|IsInLearning|HasNoDataLocks|EmployedBeforeSchemeStarted|EmployedAtStartOfApprenticeship|EmployedAt365Days",
                                        ExpiryDate = DateTime.UtcNow.AddDays(10).Date,
                                        Remove = "true|false|(optional)"
                                    },
                                    new
                                    {
                                        ValidationType = "HasDaysInLearning|IsInLearning|HasNoDataLocks|EmployedBeforeSchemeStarted|EmployedAtStartOfApprenticeship|EmployedAt365Days",
                                        ExpiryDate = DateTime.UtcNow.AddDays(15).Date,
                                        Remove = "true|false|(optional)"
                                    }
                                },
                                ServiceRequest = new ServiceRequest()
                                {
                                    TaskId = "taskId1234_optional",
                                    DecisionReference = "decisionReference123_optional",
                                    TaskCreatedDate = DateTime.UtcNow.Date
                                }
                            },
                            new
                            {
                                AccountLegalEntityId = 5678,
                                ULN = 8765,
                                ValidationSteps = new List<object>()
                                {
                                    new
                                    {
                                        ValidationType = "HasDaysInLearning|IsInLearning|HasNoDataLocks|EmployedBeforeSchemeStarted|EmployedAtStartOfApprenticeship|EmployedAt365Days",
                                        ExpiryDate = DateTime.UtcNow.AddDays(20).Date,
                                        Remove = "true|false|(optional)"
                                    }
                                },
                                ServiceRequest = new ServiceRequest()
                                {
                                    TaskId = "taskId5678_optional",
                                    DecisionReference = "decisionReference123_optional",
                                    TaskCreatedDate = DateTime.UtcNow.Date
                                }
                            }
                }
            });
        }
    }
}
