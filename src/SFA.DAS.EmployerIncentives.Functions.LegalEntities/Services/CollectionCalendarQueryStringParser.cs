using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services
{
    public static class CollectionCalendarQueryStringParser
    {
        public static CollectionCalendarUpdateRequest ParseQueryString(IDictionary<string, string> queryParameters, out string validationMessage)
        {
            var validationErrors = new List<string>();
            string academicYear = string.Empty;
            byte periodNumber = 0;
            bool active = false;

            if (!queryParameters.ContainsKey("AcademicYear"))
            {
                validationErrors.Add("AcademicYear not set");
            }
            if (!queryParameters.ContainsKey("PeriodNumber"))
            {
                validationErrors.Add("PeriodNumber not set");
            }
            if (!queryParameters.ContainsKey("Active"))
            {
                validationErrors.Add("Active not set");
            }

            if (queryParameters.ContainsKey("AcademicYear"))
            {
                academicYear = queryParameters["AcademicYear"];
                int academicYearValue;
                if (String.IsNullOrWhiteSpace(academicYear) 
                    || academicYear.Length != 4 
                    || !int.TryParse(academicYear, out academicYearValue))
                {
                    validationErrors.Add("Invalid value for AcademicYear");
                }
            }

            if (queryParameters.ContainsKey("PeriodNumber"))
            {
                var validPeriodNumber = byte.TryParse(queryParameters["PeriodNumber"], out periodNumber);
                if (!validPeriodNumber)
                {
                    validationErrors.Add("Invalid value for PeriodNumber");
                }
                if (periodNumber < 1 || periodNumber > 12)
                {
                    validationErrors.Add("Period number should be between 1 and 12");
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
            return new CollectionCalendarUpdateRequest(academicYear, periodNumber, active);
        }
    }
}
