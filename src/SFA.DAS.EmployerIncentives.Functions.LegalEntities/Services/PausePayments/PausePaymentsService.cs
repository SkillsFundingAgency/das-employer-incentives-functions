using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments.Types;

#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments
{
    public class PausePaymentsService : IPausePaymentsService
    {
        private readonly HttpClient _client;

        public PausePaymentsService(HttpClient client)
        {
            _client = client;
        }

        public async Task SetPauseStatus(PausePaymentsRequest request)
        {
            EnsureRequestIsValid(request);

            var response = await _client.PostAsJsonAsync("pause-payments", request);

            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new PausePaymentServiceException(response.StatusCode, await GetContentAsString(response));
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task<string> GetContentAsString(HttpResponseMessage response)
        {
            string content = null;
            try
            {
                content = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                // Do nothing
            }
            return content;
        }


        private void EnsureRequestIsValid(PausePaymentsRequest request)
        {
            if (request.Action == PausePaymentsAction.NotSet)
            {
                throw new ArgumentException("Action is not set or invalid", nameof(request.Action));
            }
            if (request.AccountLegalEntityId == default)
            {
                throw new ArgumentException("AccountLegalEntityId is not set", nameof(request.AccountLegalEntityId));
            }
            if (request.ULN == default)
            {
                throw new ArgumentException("ULN is not set", nameof(request.ULN));
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
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
