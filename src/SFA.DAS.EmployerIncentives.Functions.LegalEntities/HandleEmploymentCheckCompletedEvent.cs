using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleEmploymentCheckCompletedEvent
    {
        private readonly IEmploymentCheckService _employmentCheckService;
        private readonly ILogger<HandleEmploymentCheckCompletedEvent> _logger;

        public HandleEmploymentCheckCompletedEvent(
            IEmploymentCheckService employmentCheckService,
            ILogger<HandleEmploymentCheckCompletedEvent> logger)
        {
            _employmentCheckService = employmentCheckService;
            _logger = logger;
        }

        [FunctionName("HandleEmploymentCheckCompletedEvent")]
        public Task RunEvent([NServiceBusTrigger(Endpoint = EmploymentCheck.Types.QueueNames.PublishEmploymentCheckResult)] EmploymentCheck.Types.EmploymentCheckCompletedEvent message)
        {
            _logger.LogInformation(
                "HandleEmploymentCheckCompletedEvent: CorrelationId={0}, Employed={1}, ErrorType={2}, MessageSentTime=={3}",
                message.CorrelationId,
                message.EmploymentResult,
                message.ErrorType,
                message.CheckDate
            );

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
