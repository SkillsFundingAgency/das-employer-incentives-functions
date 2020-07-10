using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives;
using SFA.DAS.EmployerIncentives.Infrastructure.Configuration;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using System;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmployerIncentivesService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEmployerIncentivesService>(s =>
            {
                var settings = s.GetService<IOptions<EmployerIncentivesApi>>().Value;

                var clientBuilder = new HttpClientBuilder()
                    .WithDefaultHeaders()
                    .WithLogging(s.GetService<ILoggerFactory>());

                if (!string.IsNullOrEmpty(settings.ClientId))
                {
                    clientBuilder.WithBearerAuthorisationHeader(new AzureActiveDirectoryBearerTokenGenerator(settings));
                }

                var httpClient = clientBuilder.Build();

                httpClient.BaseAddress = new Uri(settings.ApiBaseUrl);

                return new EmployerIncentivesService(httpClient);
            });

            return serviceCollection;
        }
    }
}
