using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments.Types;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Types;
using SFA.DAS.EmployerIncentives.Types;

#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments
{
    public class PaymentsServiceValidation : IPaymentsService
    {
        private readonly IPaymentsService _paymentsService;

        public PaymentsServiceValidation(IPaymentsService paymentsService)
        {
            _paymentsService = paymentsService;
        }

        public Task SetPauseStatus(PausePaymentsRequest request)
        {
            EnsureSetPauseStatusRequestIsValid(request);

            return _paymentsService.SetPauseStatus(request);
        }

        public Task RevertPayments(RevertPaymentsRequest request)
        {
            EnsureRevertPaymentsRequestIsValid(request);

            return _paymentsService.RevertPayments(request);
        }

        private void EnsureSetPauseStatusRequestIsValid(PausePaymentsRequest request)
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

            EnsureServiceRequestIsValid(request.ServiceRequest);
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

        private void EnsureRevertPaymentsRequestIsValid(RevertPaymentsRequest request)
        {
            if (request.Payments == null || request.Payments.Count == 0)
            {
                throw new ArgumentException("Payment Ids are not set", nameof(request.Payments));
            }

            EnsureServiceRequestIsValid(request.ServiceRequest);
        }

        private void EnsureServiceRequestIsValid(ServiceRequest serviceRequest)
        {
            if (serviceRequest == null)
            {
                throw new ArgumentException("Service Request is not set", nameof(serviceRequest));
            }
            if (string.IsNullOrWhiteSpace(serviceRequest.TaskId))
            {
                throw new ArgumentException("Service Request Task Id is not set", nameof(serviceRequest.TaskId));
            }
            if (string.IsNullOrWhiteSpace(serviceRequest.DecisionReference))
            {
                throw new ArgumentException("Service Request Decision Reference is not set", nameof(serviceRequest.DecisionReference));
            }
            if (serviceRequest.TaskCreatedDate == null)
            {
                throw new ArgumentException("Service Request Task Created Date is not set", nameof(serviceRequest.TaskCreatedDate));
            }
        }
    }
}
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
