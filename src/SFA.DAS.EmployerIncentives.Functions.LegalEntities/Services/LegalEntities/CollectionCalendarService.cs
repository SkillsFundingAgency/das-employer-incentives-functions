using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class CollectionCalendarService : ICollectionCalendarService
    {
        private readonly HttpClient _client;

        public CollectionCalendarService(HttpClient client)
        {
            _client = client;
        }

        public async Task UpdatePeriod(CollectionCalendarUpdateRequest updateRequest)
        {
            var url = "collectionPeriods";
            var response = await _client.PatchAsync(url, updateRequest.GetStringContent());
            response.EnsureSuccessStatusCode();
        }
    }
}
