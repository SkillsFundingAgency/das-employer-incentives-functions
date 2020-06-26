using SFA.DAS.EmployerIncentives.Infrastructure.Commands;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.Commands.EmployerIncentiveClaimSubmitted
{
    public class EmployerIncentiveClaimSubmittedCommandValidator : IValidator<EmployerIncentiveClaimSubmittedCommand>
    {
        public Task<ValidationResult> Validate(EmployerIncentiveClaimSubmittedCommand item)
        {
            var result = new ValidationResult();

            if (item.ClaimId == default)
            {
                result.AddError("ClaimId", "Is not set");
            }

            return Task.FromResult(result);
        }
    }
}
