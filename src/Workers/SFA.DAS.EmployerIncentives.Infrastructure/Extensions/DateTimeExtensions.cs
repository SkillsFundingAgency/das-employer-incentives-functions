using System;
using System.Globalization;

namespace SFA.DAS.EmployerIncentives.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToIsoDateTime(this DateTime value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }
    }
}
