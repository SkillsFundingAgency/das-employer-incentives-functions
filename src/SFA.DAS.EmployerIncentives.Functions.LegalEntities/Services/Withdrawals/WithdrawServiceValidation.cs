using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals
{
    public class WithdrawServiceValidation : IWithdrawalService
    {
        private readonly IWithdrawalService _service;
        
        public WithdrawServiceValidation(IWithdrawalService service)
        {
            _service = service;
        }

        public async Task Withdraw(WithdrawRequest request)
        {
            EnsureRequestIsvalid(request);

            await _service.Withdraw(request);
        }

        public async Task Reinstate(ReinstateApplicationRequest request)
        {
            EnsureRequestIsvalid(request);

            await _service.Reinstate(request);
        }

        private void EnsureRequestIsvalid(WithdrawRequest request)
        {
            EnsureRequestApplicationsAreValid(request.Applications);

            if (request.WithdrawalType == WithdrawalType.NotSet)
            {
                throw new ArgumentException("WithdrawalType not set or invalid", nameof(request.WithdrawalType));
            }

            if (request.ServiceRequest == null)
            {
                throw new ArgumentException("ServiceRequest not set", nameof(request.ServiceRequest));
            }

            if (request.ServiceRequest.TaskId == default)
            {
                throw new ArgumentException("ServiceRequest TaskId not set", nameof(request.ServiceRequest.TaskId));
            }

            if (request.ServiceRequest.DecisionReference == default)
            {
                throw new ArgumentException("ServiceRequest DecisionReference not set", nameof(request.ServiceRequest.DecisionReference));
            }

            if (request.ServiceRequest.TaskCreatedDate == null)
            {
                throw new ArgumentException("ServiceRequest TaskCreatedDate not set", nameof(request.ServiceRequest.TaskCreatedDate));
            }
        }

        private void EnsureRequestApplicationsAreValid(Application[] applications)
        {
            if (applications == null || !applications.Any())
            {
                throw new ArgumentException("Applications are not set", nameof(applications));
            }

            foreach (var application in applications)
            {
                if (application.AccountLegalEntityId == default)
                {
                    throw new ArgumentException("AccountLegalEntityId not set", nameof(application.AccountLegalEntityId));
                }
                if (application.ULN == default)
                {
                    throw new ArgumentException("ULN not set", nameof(application.ULN));
                }
            }
        }

        private void EnsureRequestIsvalid(ReinstateApplicationRequest request)
        {
            foreach (var application in request.Applications)
            {
                if (application.AccountLegalEntityId == default)
                {
                    throw new ArgumentException("AccountLegalEntityId not set", nameof(application.AccountLegalEntityId));
                }

                if (application.ULN == default)
                {
                    throw new ArgumentException("ULN not set", nameof(application.ULN));
                }
            }
        }
    }

}
