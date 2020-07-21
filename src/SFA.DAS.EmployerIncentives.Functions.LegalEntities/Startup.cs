using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.EmployerIncentives.Infrastructure.Configuration;
using System;
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

            var configBuilder = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();

            if (!configuration["Environment"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
            {
                configBuilder.AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                    options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                    options.EnvironmentName = configuration["Environment"];
                    options.PreFixConfigurationKeys = false;
                });
           }
#if DEBUG
            configBuilder.AddJsonFile($"local.settings.json", optional: true);
#endif
            var config = configBuilder.Build();

            builder.Services.AddOptions();
            builder.Services.Configure<EmployerIncentivesApiOptions>(config.GetSection(EmployerIncentivesApiOptions.EmployerIncentivesApi));

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

            builder.Services.AddEmployerIncentivesService();
        }
    }
}
