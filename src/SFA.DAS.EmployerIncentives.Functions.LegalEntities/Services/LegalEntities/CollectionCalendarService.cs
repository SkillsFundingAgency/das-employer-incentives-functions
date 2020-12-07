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

        public async Task ActivatePeriod(short calendarYear, byte periodNumber, bool active)
        {
            var url = "collectionCalendar/period/active";
            var request = new { CollectionPeriodNumber = periodNumber, CollectionPeriodYear = calendarYear, Active = active };
            var response = await _client.PatchAsync(url, request.GetStringContent());
            response.EnsureSuccessStatusCode();
        }
    }
}
