using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.BlockPayments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.BlockPayments
{
    public class BlockPaymentsService : IBlockPaymentsService
    {
        private readonly HttpClient _client;

        public BlockPaymentsService(HttpClient client)
        {
            _client = client;
        }

        public async Task BlockAccountLegalEntitiesForPayments(BlockAccountLegalEntityForPaymentsRequest blockPaymentsRequest)
        {
            var response = await _client.PatchAsync("blockedpayments", blockPaymentsRequest.GetStringContent());

            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new BlockPaymentsServiceException(response.StatusCode, await GetContentAsString(response));
            }

            response.EnsureSuccessStatusCode();
        }

        private async Task<string> GetContentAsString(HttpResponseMessage response)
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

    }
}
