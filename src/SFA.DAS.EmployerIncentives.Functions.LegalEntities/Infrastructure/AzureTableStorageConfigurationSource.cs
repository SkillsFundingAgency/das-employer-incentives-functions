using Microsoft.Extensions.Configuration;
using PensionsRegulator.Functions.Infrastructure.Configuration;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Infrastructure
{
    public class AzureTableStorageConfigurationSource : IConfigurationSource
    {
        private readonly string _connection;
        private readonly string _environment;
        private readonly string _version;
        private readonly string _appName;
        public AzureTableStorageConfigurationSource(string connection, string appName, string environment, string version)
        {
            _connection = connection;
            _environment = environment;
            _version = version;
            _appName = appName;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AzureTableStorageConfigurationProvider(_connection, _appName, _environment, _version);
        }
    }
}
