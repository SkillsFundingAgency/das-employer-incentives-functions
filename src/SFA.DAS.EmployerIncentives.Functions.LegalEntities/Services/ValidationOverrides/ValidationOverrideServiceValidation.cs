using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ValidationOverrides.Types;
using SFA.DAS.EmployerIncentives.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ValidationOverrides
{
    public class ValidationOverrideServiceValidation : IValidationOverrideService
    {
        private readonly IValidationOverrideService _validationOverrideService;

        public ValidationOverrideServiceValidation(IValidationOverrideService validationOverrideService)
        {
            _validationOverrideService = validationOverrideService;
        }

        public async Task Add(IEnumerable<ValidationOverride> requests)
        {
            EnsureRequestsAreValid(requests);

            await _validationOverrideService.Add(requests);
        }

        private void EnsureRequestsAreValid(IEnumerable<ValidationOverride> requests)
        {
            requests.ToList().ForEach(r => EnsureRequestIsValid(r));

            if (requests.Select(r => new { r.AccountLegalEntityId, r.ULN }).Distinct().Count() != requests.Count())
            {
                throw new ArgumentException("Duplicate ValidationOverride entries exist. The combination of AccountLegalEntityId and ULN should be unique.");
            }
        }

        private void EnsureRequestIsValid(ValidationOverride request)
        {
            if (request.ValidationSteps == null || !request.ValidationSteps.Any())
            {
                throw new ArgumentException($"ValidationSteps not set or invalid for AccountLegalEntityId : {request.AccountLegalEntityId}, ULN : {request.ULN}");
            }

            if (request.ValidationSteps.Select(v => v.ValidationType).Distinct().Count() != request.ValidationSteps.Count())
            {
                throw new ArgumentException($"Duplicate ValidationSteps exist for AccountLegalEntityId : {request.AccountLegalEntityId}, ULN : {request.ULN}");
            }

            request.ValidationSteps.ToList().ForEach(v =>
            {
                try
                {
                    EnsureValidationStepIsValid(v);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException($"ValidationStep invalid for AccountLegalEntityId : {request.AccountLegalEntityId}, ULN : {request.ULN}, Error : {ex.Message}");
                }
            });

            if (request.AccountLegalEntityId == default)
            {
                throw new ArgumentException($"AccountLegalEntityId not set for AccountLegalEntityId : {request.AccountLegalEntityId}, ULN : {request.ULN}");
            }
            if (request.ULN == default)
            {
                throw new ArgumentException($"ULN not set for AccountLegalEntityId : {request.AccountLegalEntityId}, ULN : {request.ULN}");
            }
            if (request.ServiceRequest == null)
            {
                request.ServiceRequest = new ServiceRequest() { };
            }
            if (request.ServiceRequest.TaskCreatedDate == null)
            {
                request.ServiceRequest.TaskCreatedDate = DateTime.UtcNow;
            }
        }
        private void EnsureValidationStepIsValid(ValidationStep validationStep)
        {
            if (validationStep.ValidationType == ValidationType.NotSet)
            {
                throw new ArgumentException("ValidationType not set or invalid");
            }

            if ((!validationStep.Remove.HasValue || !validationStep.Remove.Value) && (validationStep.ExpiryDate.Date < DateTime.Today))
            {
                throw new ArgumentException($"ExpiryDate '{validationStep.ExpiryDate:yyyy'-'MM'-'dd'T'HH':'mm'Z'}' can not be in the past");
            }
        }
    }
}
