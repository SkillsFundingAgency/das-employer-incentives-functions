using Microsoft.Extensions.Configuration;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Infrastructure
{
    public static class AzureTableStorageConfigurationExtensions
    {
        public static IConfigurationBuilder AddAzureTableStorageConfiguration(this IConfigurationBuilder builder, string connection, string appName, string environment, string version)
        {
            return builder.Add(new AzureTableStorageConfigurationSource(connection, appName, environment, version));
        }
    }
}