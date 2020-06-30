using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Infrastructure.Logging;
using Microsoft.Azure.WebJobs.Host.Config;
using NServiceBus;
using SFA.DAS.NServiceBus.AzureFunction.Configuration;
using SFA.DAS.NServiceBus.AzureFunction.Hosting;
using SFA.DAS.EmployerIncentives.Infrastructure.Decorators;
using SFA.DAS.EmployerIncentives.Infrastructure.Commands;
using SFA.DAS.EmployerIncentives.Infrastructure.DistributedLock;
using SFA.DAS.EmployerIncentives.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace SFA.DAS.EmployerIncentives.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNLog(this IServiceCollection serviceCollection)
        {
            var nLogConfiguration = new NLogConfiguration();

            serviceCollection.AddLogging((options) =>
            {
                options.SetMinimumLevel(LogLevel.Trace);
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

        public static IServiceCollection AddNServiceBus(this IServiceCollection serviceCollection, ILogger logger, string testStorageDirectory = null)
        {
            serviceCollection.AddSingleton<IExtensionConfigProvider, NServiceBusExtensionConfigProvider>((c) =>
            {
                var options = new NServiceBusOptions
                {
                    OnMessageReceived = (context) =>
                    {
                        context.Headers.TryGetValue("NServiceBus.EnclosedMessageTypes", out string messageType);
                        context.Headers.TryGetValue("NServiceBus.MessageId", out string messageId);
                        context.Headers.TryGetValue("NServiceBus.CorrelationId", out string correlationId);
                        context.Headers.TryGetValue("NServiceBus.OriginatingEndpoint", out string originatingEndpoint);
                        logger.LogInformation($"Received NServiceBusTriggerData Message of type '{(messageType != null ? messageType.Split(',')[0] : string.Empty)}' with messageId '{messageId}' and correlationId '{correlationId}' from endpoint '{originatingEndpoint}'");

                    },
                    OnMessageErrored = (ex, context) =>
                    {
                        context.Headers.TryGetValue("NServiceBus.EnclosedMessageTypes", out string messageType);
                        context.Headers.TryGetValue("NServiceBus.MessageId", out string messageId);
                        context.Headers.TryGetValue("NServiceBus.CorrelationId", out string correlationId);
                        context.Headers.TryGetValue("NServiceBus.OriginatingEndpoint", out string originatingEndpoint);
                        logger.LogError(ex, $"Error handling NServiceBusTriggerData Message of type '{(messageType != null ? messageType.Split(',')[0] : string.Empty)}' with messageId '{messageId}' and correlationId '{correlationId}' from endpoint '{originatingEndpoint}'");
                    }
                };

                if(testStorageDirectory != null)
                {
                    options.EndpointConfiguration = (endpoint) =>
                    {
                        endpoint.UseTransport<LearningTransport>().StorageDirectory(testStorageDirectory);
                        return endpoint;
                    };
                }

               return new NServiceBusExtensionConfigProvider(options);
             });

            return serviceCollection;
        }

        public static IServiceCollection AddCommandHandlerDecorators(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .Decorate(typeof(ICommandHandler<>), typeof(CommandHandlerWithDistributedLock<>))
                .Decorate(typeof(ICommandHandler<>), typeof(CommandHandlerWithRetry<>))
                .Decorate(typeof(ICommandHandler<>), typeof(CommandHandlerWithValidator<>))
                .Decorate(typeof(ICommandHandler<>), typeof(CommandHandlerWithLogging<>));

            return serviceCollection;
        }

        public static IServiceCollection AddDistributedLockProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDistributedLockProvider, AzureDistributedLockProvider>(s =>
               new AzureDistributedLockProvider(
                   s.GetRequiredService<IOptions<ApplicationSettings>>(),
                   s.GetRequiredService<ILogger<AzureDistributedLockProvider>>(),
                   "employer-incentives-distributed-locks"));

            return serviceCollection;
        }
    }
}
