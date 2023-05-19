using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.RecalculateEarnings.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.RecalculateEarnings
{
    public class RecalculateEarningsService : IRecalculateEarningsService
    {
        private readonly HttpClient _client;

        public RecalculateEarningsService(HttpClient client)
        {
            _client = client;
        }

        public async Task RecalculateEarnings(RecalculateEarningsRequest recalculateEarningsRequest)
        {
            var url = "earningsRecalculations";

            var response = await _client.PostAsync(url, recalculateEarningsRequest.GetStringContent());

            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new RecalculateEarningsServiceException(response.StatusCode, await GetContentAsString(response));
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
    }
}
