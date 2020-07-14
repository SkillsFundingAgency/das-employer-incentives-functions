
namespace SFA.DAS.EmployerIncentives.Infrastructure.Configuration
{
    public class RetryPolicies
    {
        public int RetryWaitInMilliSeconds { get; set; }
        public int RetryAttempts { get; set; }
    }
}
