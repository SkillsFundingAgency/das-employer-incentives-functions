using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus;
using NServiceBus.Transport;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Hooks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.Testing.AzureStorageEmulator;
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
        public Mock<IDateTimeProvider> MockDateTimeProvider = new Mock<IDateTimeProvider>();
        public HandleRefreshLegalEntitiesRequest HttpTriggerRefreshLegalEntities { get; set; }
        public RefreshVendorRegistrationCaseStatus TimerTriggerRefreshVendorRegistrationCaseStatus { get; set; }

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
                { "NServiceBusConnectionString", "UseDevelopmentStorage=true" },
                { "AzureWebJobsStorage", "UseDevelopmentStorage=true" }
            };
        }

        public async Task Start()
        {
            var startUp = new Startup();

            var hostBuilder = new HostBuilder()
                    .ConfigureHostConfiguration(a =>
                    {
                        a.Sources.Clear();
                        a.AddInMemoryCollection(_hostConfig);
                    })
                    .ConfigureAppConfiguration(a =>
                    {
                        a.Sources.Clear();
                        a.AddInMemoryCollection(_appConfig);
                        a.SetBasePath(_testMessageBus.StorageDirectory.FullName);
                    })
                    .ConfigureWebJobs(startUp.Configure)
                ;

            _ = hostBuilder.ConfigureServices((s) =>
            {
                s.Configure<Config.EmployerIncentivesApiOptions>(a =>
                {
                    a.ApiBaseUrl = _testEmployerIncentivesApi.BaseAddress;
                    a.SubscriptionKey = "";
                });

                s.Configure<Config.FunctionConfigurationOptions>(o =>
                {
                    o.AzureWebJobsStorage = _appConfig["AzureWebJobsStorage"];
                });

                _ = s.AddNServiceBus(new LoggerFactory().CreateLogger<TestLegalEntitiesFunctions>(),
                    (o) =>
                    {
                        o.EndpointConfiguration = (endpoint) =>
                        {
                            endpoint.UseTransport<LearningTransport>().StorageDirectory(_testMessageBus.StorageDirectory.FullName);
                            return endpoint;
                        };

                        var hook = _messageHooks.SingleOrDefault(h => h is Hook<MessageContext>) as Hook<MessageContext>;
                        if (hook != null)
                        {
                            o.OnMessageReceived = (message) =>
                            {
                                hook?.OnReceived(message);
                            };
                            o.OnMessageProcessed = (message) =>
                            {
                                hook?.OnProcessed(message);
                            };
                            o.OnMessageErrored = (exception, message) =>
                            {
                                hook?.OnErrored(exception, message);
                            };
                        }
                    });
            });

            hostBuilder.UseEnvironment("LOCAL");
            host = await hostBuilder.StartAsync();

            // ideally use the test server but no functions support yet.
            HttpTriggerRefreshLegalEntities = new HandleRefreshLegalEntitiesRequest(host.Services.GetService(typeof(ILegalEntitiesService)) as ILegalEntitiesService);
            TimerTriggerRefreshVendorRegistrationCaseStatus = new RefreshVendorRegistrationCaseStatus(host.Services.GetService(typeof(IVendorRegistrationFormService)) as IVendorRegistrationFormService,
                host.Services.GetService(typeof(IVrfCaseRefreshConfiguration)) as IVrfCaseRefreshConfiguration, MockDateTimeProvider.Object);

            AzureStorageEmulatorManager.StartStorageEmulator(); // only works if emulator sits here: "C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe"
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
