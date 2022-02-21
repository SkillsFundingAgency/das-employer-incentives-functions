using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments.Types;
using System;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments
{
    public class PausePaymentsServiceValidation : IPausePaymentsService
    {
        private readonly IPausePaymentsService _pausePaymentsService;

        public PausePaymentsServiceValidation(IPausePaymentsService pausePaymentsService)
        {
            _pausePaymentsService = pausePaymentsService;
        }

        public async Task SetPauseStatus(PausePaymentsRequest request)
        {
            EnsureRequestIsValid(request);

            await _pausePaymentsService.SetPauseStatus(request);
        }
        
        private void EnsureRequestIsValid(PausePaymentsRequest request)
        {
            if (request.Applications.Select(r => new { r.AccountLegalEntityId, r.ULN }).Distinct().Count() != request.Applications.Count())
            {
                throw new ArgumentException("Duplicate application entries exist. The combination of AccountLegalEntityId and ULN should be unique.");
            }

            request.Applications.ToList().ForEach(r => EnsureApplicationIsValid(r));

            if (request.Action == PausePaymentsAction.NotSet)
            {
                throw new ArgumentException("Action is not set or invalid", nameof(request.Action));
            }
            
            if (request.ServiceRequest == null)
            {
                throw new ArgumentException("Service Request is not set", nameof(request.ServiceRequest));
            }
            if (string.IsNullOrWhiteSpace(request.ServiceRequest.TaskId))
            {
                throw new ArgumentException("Service Request Task Id is not set", nameof(request.ServiceRequest.TaskId));
            }
            if (string.IsNullOrWhiteSpace(request.ServiceRequest.DecisionReference))
            {
                throw new ArgumentException("Service Request Decision Reference is not set", nameof(request.ServiceRequest.DecisionReference));
            }
            if (request.ServiceRequest.TaskCreatedDate == null)
            {
                throw new ArgumentException("Service Request Task Created Date is not set", nameof(request.ServiceRequest.TaskCreatedDate));
            }
        }

        private void EnsureApplicationIsValid(Application application)
        {
            if (application.AccountLegalEntityId == default)
            {
                throw new ArgumentException($"AccountLegalEntityId not set for AccountLegalEntityId : {application.AccountLegalEntityId}, ULN : {application.ULN}");
            }
            if (application.ULN == default)
            {
                throw new ArgumentException($"ULN not set for AccountLegalEntityId : {application.AccountLegalEntityId}, ULN : {application.ULN}");
            }
        }
    }
}
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
