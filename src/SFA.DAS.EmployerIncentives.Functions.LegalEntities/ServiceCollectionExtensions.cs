using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals;
using SFA.DAS.EmployerIncentives.Infrastructure.Configuration;
using SFA.DAS.Http;
using System;
using System.Net.Http;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmployerIncentivesService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddClient<IJobsService>((c, s) => new JobsService(c));
            serviceCollection.AddClient<IEmploymentCheckService>((c, s) => new EmploymentCheckService(c));
            serviceCollection.Decorate<IEmploymentCheckService, EmploymentCheckServiceWithLogging>();
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
            serviceCollection.AddClient<IPausePaymentsService>((c, s) => new PausePaymentsService(c));
            serviceCollection.Decorate<IPausePaymentsService, PausePaymentsServiceValidation>();
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

        public static IServiceCollection AddNLog(this IServiceCollection serviceCollection)
        {
            var nLogConfiguration = new NLogConfiguration();

            serviceCollection.AddLogging((options) =>
            {
                options.AddFilter(typeof(Startup).Namespace, LogLevel.Information);
                options.SetMinimumLevel(LogLevel.Trace);
                options.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
                options.AddConsole();

                nLogConfiguration.ConfigureNLog();
            });

            return serviceCollection;
        }
    }
}
