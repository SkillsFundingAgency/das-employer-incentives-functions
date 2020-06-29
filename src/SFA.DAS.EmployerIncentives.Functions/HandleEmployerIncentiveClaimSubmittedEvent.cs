using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Infrastructure.Commands;
using SFA.DAS.EmployerIncentives.Functions.Commands.EmployerIncentiveClaimSubmitted;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using SFA.DAS.EmployerIncentives.Infrastructure;

namespace SFA.DAS.EmployerIncentives.Functions
{
    public class HandleEmployerIncentiveClaimSubmittedEvent
    {
        private readonly ICommandHandler<EmployerIncentiveClaimSubmittedCommand> _handler;

        public HandleEmployerIncentiveClaimSubmittedEvent(ICommandHandler<EmployerIncentiveClaimSubmittedCommand> handler)
        {
            _handler = handler;
        }


        [FunctionName("HandleEmployerIncentiveClaimSubmittedEvent")]
        public async Task Run([NServiceBusTrigger(Endpoint = QueueNames.EmployerIncentiveClaimSubmitted)] EmployerIncentiveClaimSubmittedEvent message)
        {
            await _handler.Handle(new EmployerIncentiveClaimSubmittedCommand(message.IncentiveClaimApprenticeshipId));
        }
    }
}
