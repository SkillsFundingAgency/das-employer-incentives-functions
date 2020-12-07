using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleActivateCollectionCalendarPeriod
    {
        private readonly ICollectionCalendarService _collectionCalendarService;

        public HandleActivateCollectionCalendarPeriod(ICollectionCalendarService collectionCalendarService)
        {
            _collectionCalendarService = collectionCalendarService;
        }

        [FunctionName("HttpActivateCollectionCalendarPeriod")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function)] HttpRequest request, ILogger log)
        {
            var queryParameters = request.GetQueryParameterDictionary();
            if (!queryParameters.ContainsKey("CalendarYear") 
             || !queryParameters.ContainsKey("PeriodNumber")
             || !queryParameters.ContainsKey("Active"))
            {
                return new BadRequestResult();
            }
            short calendarYear;
            var validCalendarYear = short.TryParse(queryParameters["CalendarYear"], out calendarYear);
            byte periodNumber;
            var validPeriodNumber = byte.TryParse(queryParameters["PeriodNumber"], out periodNumber);
            bool active;
            var validActive = bool.TryParse(queryParameters["Active"], out active);
            if (!validCalendarYear || !validPeriodNumber || !validActive)
            {
                return new BadRequestResult();
            }
            

            log.LogInformation($"Started activate collection calendar period for calendar year {calendarYear} period {periodNumber} set active = {active}");
            await _collectionCalendarService.ActivatePeriod(calendarYear, periodNumber, active);
            log.LogInformation($"Completed activate collection calendar period for calendar year {calendarYear} period {periodNumber} set active = {active}");

            return new OkResult();
        }

    }
}
