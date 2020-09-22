namespace SFA.DAS.EmployerIncentives.Infrastructure.Configuration
{
    public class FunctionConfigurationOptions
    {
        public const string EmployerIncentivesFunctionsConfiguration = "EmployerIncentivesFunctions";
        public virtual string AllowedHashstringCharacters { get; set; }
        public virtual string Hashstring { get; set; }
        public virtual string AzureWebJobsStorage { get; set; }
    }
}
