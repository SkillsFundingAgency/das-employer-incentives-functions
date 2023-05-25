using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.BlockPayments;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Payments;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.RecalculateEarnings;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.ValidationOverrides;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals;
using SFA.DAS.EmployerIncentives.Support.Infrastructure.Configuration;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerIncentives.Functions.Support
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
