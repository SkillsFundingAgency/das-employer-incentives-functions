using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.EmployerIncentives.Messages.Events;
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
        public Task RunEvent([NServiceBusTrigger(Endpoint = QueueNames.EmploymentCheckCompleted)] EmploymentCheckCompletedEvent message)
        {
            var updateRequest = new UpdateRequest
            {
                CorrelationId = message.CorrelationId,
                Result = Map(message.Result).ToString(),
                DateChecked = message.DateChecked 
            };
            
            return _employmentCheckService.Update(updateRequest);
        }

        private EmploymentCheckResult Map(string result)
        {
            return result.ToLower() switch
            {
                "employed" => EmploymentCheckResult.Employed,
                "not employed" => EmploymentCheckResult.NotEmployed,
                "hmrc unknown" => EmploymentCheckResult.HMRCUnknown,
                "no nino found" => EmploymentCheckResult.NoNINOFound,
                "no account found" => EmploymentCheckResult.NoAccountFound,
                _ => throw new ArgumentException($"Unexpected Employment Check result reveived : {result}"),
            };
        }
    }
}
