using SFA.DAS.EmployerIncentives.Infrastructure.Commands;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.Commands.EmployerIncentiveClaimSubmitted
{
    public class EmployerIncentiveClaimSubmittedCommandValidator : IValidator<EmployerIncentiveClaimSubmittedCommand>
    {
        public Task<ValidationResult> Validate(EmployerIncentiveClaimSubmittedCommand item)
        {
            var result = new ValidationResult();

            if (item.IncentiveClaimApprenticeshipId == default)
            {
                result.AddError("IncentiveClaimApprenticeshipId", "Is not set");
            }

            return Task.FromResult(result);
        }
    }
}
