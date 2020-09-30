using Microsoft.WindowsAzure.Storage.Table;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Infrastructure
{
    public class ConfigurationItem : TableEntity
    {
        public string Data { get; set; }
    }
}
