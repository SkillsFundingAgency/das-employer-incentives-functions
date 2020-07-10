using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.EmployerIncentives.Functions.Commands.AddLegalEntity;
using SFA.DAS.EmployerIncentives.Functions.Commands.EmployerIncentiveClaimSubmitted;
using SFA.DAS.EmployerIncentives.Handlers;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.EmployerIncentives.Infrastructure.ApiClient;
using SFA.DAS.EmployerIncentives.Infrastructure.Commands;
using SFA.DAS.EmployerIncentives.Infrastructure.Configuration;
using SFA.DAS.EmployerIncentives.Infrastructure.DistributedLock;
using System;
using System.IO;

[assembly: FunctionsStartup(typeof(SFA.DAS.EmployerIncentives.Functions.Startup))]
namespace SFA.DAS.EmployerIncentives.Functions
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddNLog();

            var serviceProvider = builder.Services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                    options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                    options.EnvironmentName = configuration["Environment"];
                    options.PreFixConfigurationKeys = false;
                })
                .Build();


            builder.AddExecutionContextBinding();
            var logger = serviceProvider.GetService<ILoggerProvider>().CreateLogger(GetType().AssemblyQualifiedName);

            if (configuration["Environment"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
            {
                builder.Services.AddNServiceBus(logger, Path.Combine(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().IndexOf("src")), @"src\SFA.DAS.EmployerIncentives.Functions.TestConsole\.learningtransport"));
            }
            else
            {
                builder.Services.AddNServiceBus(logger);
            }

            builder.Services.AddOptions();
            builder.Services.Configure<ApplicationSettings>(config.GetSection("ApplicationSettings"));
            builder.Services.Configure<RetryPolicies>(config.GetSection("RetryPolicies"));
            builder.Services.Configure<ClientApiConfiguration>(config.GetSection("ClientApiSettings"));

            builder.Services.AddSingleton<IDistributedLockProvider, AzureDistributedLockProvider>(s =>
                new AzureDistributedLockProvider(
                    s.GetRequiredService<IOptions<ApplicationSettings>>(),
                    s.GetRequiredService<ILogger<AzureDistributedLockProvider>>(),
                    "employer-incentives-distributed-locks"));
            builder.Services.AddSingleton<IValidator<AddLegalEntityCommand>, AddLegalEntityCommandValidator>();
            builder.Services.AddSingleton<IValidator<EmployerIncentiveClaimSubmittedCommand>, EmployerIncentiveClaimSubmittedCommandValidator>();
            builder.Services.AddSingleton(c => new Policies(c.GetService<IOptions<RetryPolicies>>()));

            builder.Services.AddTransient<ICommandHandler<EmployerIncentiveClaimSubmittedCommand>, EmployerIncentiveClaimSubmittedCommandHandler>();

            builder.Services.AddTransient<ICalculatePaymentsApiClientFactory, CalculatePaymentsApiClientFactory>();
            builder.Services.AddSingleton<ICalculatePaymentApiClient>(x => x.GetService<ICalculatePaymentsApiClientFactory>().CreateClient());

        }
    }
}
