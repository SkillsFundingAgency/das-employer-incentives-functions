using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using SFA.DAS.EmployerIncentives.Infrastructure.Configuration;
using System;

namespace SFA.DAS.EmployerIncentives.Infrastructure.Commands
{
    public class Policies
    {
        public readonly AsyncRetryPolicy RetryPolicy;

        public Policies(IOptions<RetryPolicies> settings)
        {
            var retryPolicies = settings.Value;

            RetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryPolicies.RetryAttempts,
                    retryAttempt => TimeSpan.FromMilliseconds(retryPolicies.RetryWaitInMilliSeconds));
        }
    }
}
