using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Transport;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Hooks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives;
using SFA.DAS.EmployerIncentives.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Config = SFA.DAS.EmployerIncentives.Infrastructure.Configuration;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services
{
    public class TestLegalEntitiesFunctions : IDisposable
    {
        private readonly TestEmployerIncentivesApi _testEmployerIncentivesApi;
        private readonly Dictionary<string, string> _appConfig;
        private readonly Dictionary<string, string> _hostConfig;
        private readonly TestMessageBus _testMessageBus;
        private readonly List<IHook> _messageHooks;
        private IHost host;
        private bool isDisposed;
        public HandleRefreshLegalEntitiesRequest HttpTriggerRefreshLegalEntities { get; set;}

        public TestLegalEntitiesFunctions(
            TestEmployerIncentivesApi testEmployerIncentivesApi, 
            TestMessageBus testMessageBus,
            List<IHook> messageHooks)
        {
            _testEmployerIncentivesApi = testEmployerIncentivesApi;
            _testMessageBus = testMessageBus;
            _messageHooks = messageHooks;

            _hostConfig = new Dictionary<string, string>();
            _appConfig = new Dictionary<string, string>
            {
                { "EnvironmentName", "LOCAL" },
                { "ConfigurationStorageConnectionString", "UseDevelopmentStorage=true" },
                { "ConfigNames", "SFA.DAS.EmployerIncentives.Functions" },
                { "Values:AzureWebJobsStorage", "UseDevelopmentStorage=true" }
            };
        }

        public async Task Start()
        {
            var startUp = new Startup();

            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration(a =>
                {
                    a.AddInMemoryCollection(_hostConfig);
                })
                .ConfigureAppConfiguration(a =>
                {
                    a.AddInMemoryCollection(_appConfig);
                    a.SetBasePath(_testMessageBus.StorageDirectory.FullName);
                })
               .ConfigureWebJobs(startUp.Configure);

            _ = hostBuilder.ConfigureServices((s) =>
            {
                s.Configure<Config.EmployerIncentivesApi>(a =>
                {
                    a.ApiBaseUrl = _testEmployerIncentivesApi.BaseAddress;
                    a.ClientId = "";
                });

                s.AddNServiceBus(new LoggerFactory().CreateLogger<TestLegalEntitiesFunctions>(),
                    (o) =>
                    {
                        o.EndpointConfiguration = (endpoint) =>
                        {
                            
                            endpoint.UseTransport<LearningTransport>().StorageDirectory(_testMessageBus.StorageDirectory.FullName);
                            return endpoint;
                        };
                        o.OnMessageReceived = (message) =>
                        {
                            var hook = _messageHooks.SingleOrDefault(h => h is Hook<MessageContext>);                            
                            if (hook != null)
                            {
                                (hook as Hook<MessageContext>)?.OnReceived(message);
                            }
                        };
                    });
            });

            hostBuilder.UseEnvironment("LOCAL");

            host = await hostBuilder.StartAsync();            

            // ideally use the test server but no functions support yet.
            HttpTriggerRefreshLegalEntities = new HandleRefreshLegalEntitiesRequest(host.Services.GetService(typeof(IEmployerIncentivesService)) as IEmployerIncentivesService);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing && host != null)
            {
                host.StopAsync();                
            }

            isDisposed = true;
        }
    }
}
