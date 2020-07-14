using SFA.DAS.EmployerIncentives.Infrastructure.Commands;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Infrastructure.Decorators
{
    public class CommandHandlerWithRetry<T> : ICommandHandler<T> where T : ICommand
    {
        private readonly ICommandHandler<T> _handler;
        private readonly Policies _policies;

        public CommandHandlerWithRetry(
            ICommandHandler<T> handler,
            Policies policies)
        {
            _handler = handler;
            _policies = policies;
        }

        public Task Handle(T command)
        {
            return _policies.RetryPolicy.ExecuteAsync(() => _handler.Handle(command));
        }
    }
}
