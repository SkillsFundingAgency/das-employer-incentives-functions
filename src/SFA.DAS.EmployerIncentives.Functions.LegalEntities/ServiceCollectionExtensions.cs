using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
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

            serviceCollection.AddClient<ICollectionCalendarService>((c, s) => new CollectionCalendarService(c));
            serviceCollection.Decorate<ICollectionCalendarService, CollectionCalendarServiceWithLogging>();

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
    }
}
