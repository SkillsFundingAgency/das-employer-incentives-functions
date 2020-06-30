using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Infrastructure.Commands
{
    public interface IValidator<T>
    {
        Task<ValidationResult> Validate(T item);
    }
}
