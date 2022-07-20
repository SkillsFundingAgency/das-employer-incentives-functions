using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals.Types;
using SFA.DAS.EmployerIncentives.Types;
using System;
using System.Net.Http;
using System.Threading.Tasks;

#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals
{
    public class WithdrawalService : IWithdrawalService
    {
        private readonly HttpClient _client;

        public WithdrawalService(HttpClient client)
        {
            _client = client;
        }

        public async Task Withdraw(WithdrawRequest request)
        {
            EnsureRequestIsvalid(request);

            var response = await _client.PostAsJsonAsync("withdrawals", request);

            response.EnsureSuccessStatusCode();
        }

        public async Task Reinstate(ReinstateApplicationRequest request)
        {
            EnsureRequestIsvalid(request);

            var response = await _client.PostAsJsonAsync("withdrawal-reinstatements", request);

            response.EnsureSuccessStatusCode();
        }

        private void EnsureRequestIsvalid(WithdrawRequest request)
        {
            if (request.WithdrawalType == WithdrawalType.NotSet)
            {
                throw new ArgumentException("WithdrawalType not set or invalid", nameof(request.WithdrawalType));
            }
            if (request.AccountLegalEntityId == default)
            {
                throw new ArgumentException("AccountLegalEntityId not set", nameof(request.AccountLegalEntityId));
            }
            if (request.ULN == default)
            {
                throw new ArgumentException("ULN not set", nameof(request.ULN));
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

                if (application.ServiceRequest == null)
                {
                    throw new ArgumentException($"Service Request is not set for application with ULN {application.ULN}", nameof(application.ServiceRequest));
                }
                if (string.IsNullOrWhiteSpace(application.ServiceRequest.TaskId))
                {
                    throw new ArgumentException("Service Request Task Id is not set for application with ULN {application.ULN}", nameof(application.ServiceRequest.TaskId));
                }
                if (string.IsNullOrWhiteSpace(application.ServiceRequest.DecisionReference))
                {
                    throw new ArgumentException("Service Request Decision Reference is not set for application with ULN {application.ULN}", nameof(application.ServiceRequest.DecisionReference));
                }
                if (application.ServiceRequest.TaskCreatedDate == null)
                {
                    throw new ArgumentException("Service Request Task Created Date is not set for application with ULN {application.ULN}", nameof(application.ServiceRequest.TaskCreatedDate));
                }
            }
        }
    }
}
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
