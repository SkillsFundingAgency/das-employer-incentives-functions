using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Infrastructure.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task Handle(T command);
    }
}
