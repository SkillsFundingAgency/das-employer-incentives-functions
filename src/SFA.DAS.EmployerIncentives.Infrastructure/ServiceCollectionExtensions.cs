﻿using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Infrastructure.Logging;
using SFA.DAS.NServiceBus.AzureFunction.Configuration;
using SFA.DAS.NServiceBus.AzureFunction.Hosting;
using System;

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

        public static IServiceCollection AddNServiceBus(
            this IServiceCollection serviceCollection,
            ILogger logger,
            Action<NServiceBusOptions> OnConfigureOptions = null)
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

                if(OnConfigureOptions != null)
                {
                    OnConfigureOptions.Invoke(options);
                }                

               return new NServiceBusExtensionConfigProvider(options);
             });

            return serviceCollection;
        }       
    }
}