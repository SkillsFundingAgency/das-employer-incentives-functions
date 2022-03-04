using System;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RecalculateEarnings.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RecalculateEarnings
{
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one
    public class RecalculateEarningsServiceValidation : IRecalculateEarningsService
    {
        private readonly IRecalculateEarningsService _recalculateEarningsService;

        public RecalculateEarningsServiceValidation(IRecalculateEarningsService recalculateEarningsService)
        {
            _recalculateEarningsService = recalculateEarningsService;
        }

        public async Task RecalculateEarnings(RecalculateEarningsRequest recalculateEarningsRequest)
        {
            EnsureRequestIsValid(recalculateEarningsRequest);

            await _recalculateEarningsService.RecalculateEarnings(recalculateEarningsRequest);
        }

        private void EnsureRequestIsValid(RecalculateEarningsRequest recalculateEarningsRequest)
        {
            if (recalculateEarningsRequest.IncentiveLearnerIdentifiers == null || recalculateEarningsRequest.IncentiveLearnerIdentifiers.Count == 0)
            {
                throw new ArgumentException("Incentive Learner Identifiers are not set", nameof(recalculateEarningsRequest.IncentiveLearnerIdentifiers));
            }
            foreach(var identifier in recalculateEarningsRequest.IncentiveLearnerIdentifiers)
            {
                EnsureIncentiveLearnerIdentifierIsValid(identifier);
            }
        }

        private void EnsureIncentiveLearnerIdentifierIsValid(IncentiveLearnerIdentifierDto identifier)
        {
            if (identifier.AccountLegalEntityId == default)
            {
                throw new ArgumentException("Incentive Learner Identifier Account Legal Entity not set");
            }
            if (identifier.ULN == default)
            {
                throw new ArgumentException("Incentive Learner Identifier ULN not set");
            }
        }
    }
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one
}
