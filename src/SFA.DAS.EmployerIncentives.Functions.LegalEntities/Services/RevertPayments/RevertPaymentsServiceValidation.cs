using System;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RevertPayments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RevertPayments
{
    public class RevertPaymentsServiceValidation : IRevertPaymentsService
    {
        private readonly IRevertPaymentsService _revertPaymentsService;

        public RevertPaymentsServiceValidation(IRevertPaymentsService revertPaymentsService)
        {
            _revertPaymentsService = revertPaymentsService;
        }

        public async Task RevertPayments(RevertPaymentsRequest request)
        {
            EnsureRequestIsValid(request);

            await _revertPaymentsService.RevertPayments(request);
        }

        private void EnsureRequestIsValid(RevertPaymentsRequest request)
        {
            if (request.Payments.Count == 0)
            {
                throw new ArgumentException("Payment Ids are not set", nameof(request.Payments));
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
    }
}
