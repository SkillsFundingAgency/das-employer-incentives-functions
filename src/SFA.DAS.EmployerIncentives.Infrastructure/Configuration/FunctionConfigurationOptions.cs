namespace SFA.DAS.EmployerIncentives.Infrastructure.Configuration
{
    public class FunctionConfigurationOptions
    {
        public const string EmployerIncentivesFunctionsConfiguration = "EmployerIncentivesFunctions";
        public virtual string AzureWebJobsStorage { get; set; }
    }
}
