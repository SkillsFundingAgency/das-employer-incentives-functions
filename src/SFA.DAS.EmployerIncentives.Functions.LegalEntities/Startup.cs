using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Infrastructure;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.EmployerIncentives.Infrastructure.Configuration;
using System.IO;

[assembly: FunctionsStartup(typeof(SFA.DAS.EmployerIncentives.Functions.LegalEntities.Startup))]
namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddNLog();

            var serviceProvider = builder.Services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            var configurationBuilder = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();

            builder.AddConfiguration((configBuilder) =>
            {
                var tempConfig = configBuilder
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .Build();

                return configBuilder
                    .AddAzureTableStorageConfiguration(
                        tempConfig["ConfigurationStorageConnectionString"],
                        tempConfig["ConfigNames"],
                        tempConfig["EnvironmentName"],
                        tempConfig["ConfigurationVersion"])
                    .Build();
            });

            var config = configurationBuilder.Build();

            builder.Services.AddOptions();
            builder.Services.Configure<EmployerIncentivesApiOptions>(config.GetSection(EmployerIncentivesApiOptions.EmployerIncentivesApi));
            builder.Services.Configure<FunctionConfigurationOptions>(config.GetSection(FunctionConfigurationOptions.EmployerIncentivesFunctionsConfiguration));

            var logger = serviceProvider.GetService<ILoggerProvider>().CreateLogger(GetType().AssemblyQualifiedName);
            if (config["NServiceBusConnectionString"] == "UseDevelopmentStorage=true")
            {
                builder.Services.AddNServiceBus(logger, (options) =>
                {
                    options.EndpointConfiguration = (endpoint) =>
                    {
                        endpoint.UseTransport<LearningTransport>().StorageDirectory(Path.Combine(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().IndexOf("src")), @"src\SFA.DAS.EmployerIncentives.Functions.TestConsole\.learningtransport"));
                        return endpoint;
                    };
                });
            }
            else
            {
                builder.Services.AddNServiceBus(logger);
            }

            builder.Services
                .AddEmployerIncentivesService()
                .AddVrfCaseRefreshConfiguration()
                ;
        }
    }
}
