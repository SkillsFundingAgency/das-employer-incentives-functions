using SFA.DAS.Http.Configuration;

namespace SFA.DAS.EmployerIncentives.Infrastructure.Configuration
{
    public class ClientApiConfiguration : IAzureActiveDirectoryClientConfiguration
    {
        public string ApiBaseUrl { get; set; }

        public string Tenant { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string IdentifierUri { get; set; }
    }
}
