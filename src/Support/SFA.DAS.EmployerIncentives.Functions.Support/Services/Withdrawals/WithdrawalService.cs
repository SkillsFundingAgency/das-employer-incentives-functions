using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals.Types;

#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals
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
            var response = await _client.PostAsJsonAsync("withdrawals", request);

            response.EnsureSuccessStatusCode();
        }

        public async Task Reinstate(ReinstateApplicationRequest request)
        {
            var response = await _client.PostAsJsonAsync("withdrawal-reinstatements", request);

            response.EnsureSuccessStatusCode();
        }
    }
}
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
