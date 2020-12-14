using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services
{
    public static class CollectionCalendarQueryStringParser
    {
        public static CollectionCalendarUpdateRequest ParseQueryString(IDictionary<string, string> queryParameters, out string validationMessage)
        {
            var validationErrors = new List<string>();
            short calendarYear = 0;
            byte periodNumber = 0;
            bool active = false;

            if (!queryParameters.ContainsKey("CalendarYear"))
            {
                validationErrors.Add("CalendarYear not set");
            }
            if (!queryParameters.ContainsKey("PeriodNumber"))
            {
                validationErrors.Add("PeriodNumber not set");
            }
            if (!queryParameters.ContainsKey("Active"))
            {
                validationErrors.Add("Active not set");
            }

            if (queryParameters.ContainsKey("CalendarYear"))
            {
                var validCalendarYear = short.TryParse(queryParameters["CalendarYear"], out calendarYear);
                if (!validCalendarYear)
                {
                    validationErrors.Add("Invalid value for CalendarYear");
                }
            }

            if (queryParameters.ContainsKey("PeriodNumber"))
            {
                var validPeriodNumber = byte.TryParse(queryParameters["PeriodNumber"], out periodNumber);
                if (!validPeriodNumber)
                {
                    validationErrors.Add("Invalid value for PeriodNumber");
                }
            }

            if (queryParameters.ContainsKey("Active"))
            {
                var validActive = bool.TryParse(queryParameters["Active"], out active);
                if (!validActive)
                {
                    validationErrors.Add("Invalid value for Active");
                }
            }

            if (validationErrors.Any())
            {
                validationMessage = string.Join(". ", validationErrors);
                return null;
            }

            validationMessage = string.Empty;
            return new CollectionCalendarUpdateRequest(calendarYear, periodNumber, active);
        }
    }
}
