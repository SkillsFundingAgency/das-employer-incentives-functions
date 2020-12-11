using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawls.Types;
using System;
using System.Net.Http;
using System.Threading.Tasks;
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawls
{
    public class WithdrawlService : IWithdrawlService
    {
        private readonly HttpClient _client;

        public WithdrawlService(HttpClient client)
        {
            _client = client;
        }

        public async Task Withdraw(WithdrawRequest request)
        {
            EnsureRequestIsvalid(request);

            var response = await _client.PostAsJsonAsync("withdrawls", request);

            response.EnsureSuccessStatusCode();
        }

        private void EnsureRequestIsvalid(WithdrawRequest request)
        {
            if (request.Type == WithdrawlType.NotSet)
            {
                throw new ArgumentException("Type not set or invalid", nameof(request.Type));
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
                request.ServiceRequest.TaskCreatedDate = DateTime.Now;
            }
        }
    }
}
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
