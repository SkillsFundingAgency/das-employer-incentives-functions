using NServiceBus.Transport;
using System;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Hooks
{
    public class FunctionHooks
    {
        public Action<MessageContext> OnMessageReceived { get; set; }
        public Action<MessageContext> OnMessageProcesses { get; set; }
        public Action<Exception, MessageContext> OnMessageErrored { get; set; }
    }
}
