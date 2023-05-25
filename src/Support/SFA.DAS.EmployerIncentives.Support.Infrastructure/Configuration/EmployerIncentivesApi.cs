using SFA.DAS.Http.Configuration;

namespace SFA.DAS.EmployerIncentives.Support.Infrastructure.Configuration
{
    public class EmployerIncentivesApiOptions : IApimClientConfiguration
    {
        public const string EmployerIncentivesApi = "EmployerIncentivesApi";
        public string ApiBaseUrl { get; set; }
        public string SubscriptionKey { get; set; }
        public string ApiVersion { get; set; }
    }
}
