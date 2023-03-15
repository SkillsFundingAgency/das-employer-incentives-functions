using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RecalculateEarnings;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ValidationOverrides;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals;
using SFA.DAS.EmployerIncentives.Infrastructure.Configuration;
using SFA.DAS.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmployerIncentivesService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddClient<IJobsService>((c, s) => new JobsService(c));
            serviceCollection.AddClient<IEmploymentCheckService>((c, s) => new EmploymentCheckService(c));
            serviceCollection.Decorate<IEmploymentCheckService, EmploymentCheckServiceWithLogging>();
            serviceCollection.Decorate<IEmploymentCheckService, EmploymentCheckValidation>();
            serviceCollection.AddClient<ILegalEntitiesService>((c, s) => new LegalEntitiesService(c));
            serviceCollection.AddSingleton<IVrfCaseRefreshRepository>(
                c =>
                {
                    var settings = c.GetService<IConfiguration>();
                    return new VrfCaseRefreshRepository(settings.GetWebJobsConnectionString("AzureWebJobsStorage"), settings.GetValue<string>("EnvironmentName"));
                });

            serviceCollection.AddClient<IVendorRegistrationFormService>((c, s) => new VendorRegistrationFormService(c));
            serviceCollection.Decorate<IVendorRegistrationFormService, VendorRegistrationFormServiceWithLogging>();
            serviceCollection.AddSingleton<IVrfCaseRefreshService, VrfCaseRefreshService>();
            serviceCollection.Decorate<IVrfCaseRefreshService, VrfCaseRefreshServiceWithLogging>();

            serviceCollection.AddClient<IAgreementsService>((c, s) => new AgreementsService(c));

            serviceCollection.AddClient<IEmployerVendorIdService>((c, s) => new EmployerVendorIdService(c));
            serviceCollection.Decorate<IEmployerVendorIdService, EmployerVendorIdServiceWithLogging>();

            serviceCollection.AddClient<IEarningsResilienceCheckService>((c, s) => new EarningsResilienceCheckService(c));
            serviceCollection.Decorate<IEarningsResilienceCheckService, EarningsResilienceCheckServiceWithLogging>();

            serviceCollection.AddClient<IEmailService>((c, s) => new EmailService(c));
            serviceCollection.Decorate<IEmailService, EmailServiceWithLogging>();
            serviceCollection.AddClient<ICollectionCalendarService>((c, s) => new CollectionCalendarService(c));
            serviceCollection.Decorate<ICollectionCalendarService, CollectionCalendarServiceWithLogging>();

            serviceCollection.AddClient<IWithdrawalService>((c, s) => new WithdrawalService(c));
            serviceCollection.Decorate<IWithdrawalService, WithdrawServiceValidation>();
            serviceCollection.AddClient<IPaymentsService>((c, s) => new PaymentsService(c));
            serviceCollection.Decorate<IPaymentsService, PaymentsServiceValidation>();
            serviceCollection.AddClient<IBlockPaymentsService>((c, s) => new BlockPaymentsService(c));
            serviceCollection.Decorate<IBlockPaymentsService, BlockPaymentsServiceWithLogging>();
            serviceCollection.Decorate<IBlockPaymentsService, BlockPaymentsServiceValidation>();
            serviceCollection.AddClient<IRecalculateEarningsService>((c, s) => new RecalculateEarningsService(c));
            serviceCollection.Decorate<IRecalculateEarningsService, RecalculateEarningsServiceValidation>();
            serviceCollection.Decorate<IRecalculateEarningsService, RecalculateEarningsServiceWithLogging>();
            serviceCollection.AddClient<IValidationOverrideService>((c, s) => new ValidationOverrideService(c));
            serviceCollection.Decorate<IValidationOverrideService, ValidationOverrideServiceValidation>();

            return serviceCollection;
        }

        private static IServiceCollection AddClient<T>(this IServiceCollection serviceCollection, Func<HttpClient, IServiceProvider, T> instance) where T : class
        {
            serviceCollection.AddTransient(s =>
            {
                var settings = s.GetService<IOptions<EmployerIncentivesApiOptions>>().Value;

                var clientBuilder = new HttpClientBuilder()
                    .WithDefaultHeaders()
                    .WithApimAuthorisationHeader(settings)
                    .WithLogging(s.GetService<ILoggerFactory>());

                var httpClient = clientBuilder.Build();

                if (!settings.ApiBaseUrl.EndsWith("/"))
                {
                    settings.ApiBaseUrl += "/";
                }
                httpClient.BaseAddress = new Uri(settings.ApiBaseUrl);

                return instance.Invoke(httpClient, s);
            });

            return serviceCollection;
        }

        public static IServiceCollection AddNLog(this IServiceCollection serviceCollection, IConfiguration configuration)
        {

            var env = Environment.GetEnvironmentVariable("EnvironmentName");
            var configFileName = "nlog.config";
            if (string.IsNullOrEmpty(env) || env.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
            {
                configFileName = "nlog.local.config";
            }
            var rootDirectory = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ".."));
            var configFilePath = Directory.GetFiles(rootDirectory, configFileName, SearchOption.AllDirectories)[0];
            LogManager.Setup()
                .SetupExtensions(e => e.AutoLoadExtensions())
                .LoadConfigurationFromFile(configFilePath, optional: false)
                .LoadConfiguration(builder => builder.LogFactory.AutoShutdown = false)
                .GetCurrentClassLogger();

            serviceCollection.AddLogging((options) =>
            {
                options.AddFilter("SFA.DAS", LogLevel.Information); // this is because all logging is filtered out by default
                options.SetMinimumLevel(LogLevel.Trace);
                options.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
#if DEBUG
                options.AddConsole();
#endif
            });

            return serviceCollection;
        }
    }
}
