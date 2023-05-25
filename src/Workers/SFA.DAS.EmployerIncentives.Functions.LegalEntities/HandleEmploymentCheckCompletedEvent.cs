using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleEmploymentCheckCompletedEvent
    {
        private readonly IEmploymentCheckService _employmentCheckService;

        public HandleEmploymentCheckCompletedEvent(IEmploymentCheckService employmentCheckService)
        {
            _employmentCheckService = employmentCheckService;
        }

        [FunctionName("HandleEmploymentCheckCompletedEvent")]
        public Task RunEvent([NServiceBusTrigger(Endpoint = EmploymentCheck.Types.QueueNames.PublishEmploymentCheckResult)] EmploymentCheck.Types.EmploymentCheckCompletedEvent message)
        {
            var updateRequest = new UpdateRequest
            {
                CorrelationId = message.CorrelationId,
                Result = Map(message.EmploymentResult, message.ErrorType).ToString(),
                DateChecked = message.CheckDate
            };

            return _employmentCheckService.Update(updateRequest);
        }

        private EmploymentCheckResult Map(bool? result, string errorType)
        {
            if(result.HasValue)
            {
                if(!string.IsNullOrEmpty(errorType))
                {
                    throw new ArgumentException($"Unexpected Error type set when employmentResult is set : {errorType}");
                }
                return result.Value ? EmploymentCheckResult.Employed : EmploymentCheckResult.NotEmployed;
            }

            return errorType.ToLower() switch
            {
                "ninonotfound" => EmploymentCheckResult.NinoNotFound,
                "ninofailure" => EmploymentCheckResult.NinoFailure,
                "ninoinvalid" => EmploymentCheckResult.NinoInvalid,
                "payenotfound" => EmploymentCheckResult.PAYENotFound,
                "payefailure" => EmploymentCheckResult.PAYEFailure,
                "ninoandpayenotfound" => EmploymentCheckResult.NinoAndPAYENotFound,
                "hmrcfailure" => EmploymentCheckResult.HmrcFailure,
                _ => throw new ArgumentException($"Unexpected Employment Check result received : {errorType}"),
            };
        }
    }
}
