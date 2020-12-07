using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class CollectionCalendarServiceWithLogging : ICollectionCalendarService
    {
        private readonly ICollectionCalendarService _collectionCalendarService;
        private readonly ILogger<CollectionCalendarServiceWithLogging> _logger;

        public CollectionCalendarServiceWithLogging(
            ICollectionCalendarService collectionCalendarService,
            ILogger<CollectionCalendarServiceWithLogging> logger)
        {
            _collectionCalendarService = collectionCalendarService;
            _logger = logger;
        }

        public async Task ActivatePeriod(short calendarYear, byte periodNumber, bool active)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"Calling ICollectionCalendarService.ActivatePeriod");

                await _collectionCalendarService.ActivatePeriod(calendarYear, periodNumber, active);

                _logger.Log(LogLevel.Information, $"Called ICollectionCalendarService.ActivatePeriod");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling ICollectionCalendarService.ActivatePeriod");

                throw;
            }
        }
    }
}
