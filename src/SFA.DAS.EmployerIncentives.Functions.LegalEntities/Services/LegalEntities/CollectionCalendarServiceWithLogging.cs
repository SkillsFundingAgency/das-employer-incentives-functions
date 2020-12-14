using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
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

        public async Task UpdatePeriod(CollectionCalendarUpdateRequest collectionCalendarUpdateRequest)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"Calling ICollectionCalendarService.ActivatePeriod for period {collectionCalendarUpdateRequest.CollectionPeriodNumber} calendar year {collectionCalendarUpdateRequest.CollectionPeriodYear} active = {collectionCalendarUpdateRequest.Active} "); 
                await _collectionCalendarService.UpdatePeriod(collectionCalendarUpdateRequest);

                _logger.Log(LogLevel.Information, $"Called ICollectionCalendarService.ActivatePeriod for period {collectionCalendarUpdateRequest.CollectionPeriodNumber} calendar year {collectionCalendarUpdateRequest.CollectionPeriodYear} active = {collectionCalendarUpdateRequest.Active} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling ICollectionCalendarService.ActivatePeriod");

                throw;
            }
        }
    }
}
