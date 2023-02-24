using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Transport;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Hooks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ValidationOverrides;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Withdrawals;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.Testing.AzureStorageEmulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Payments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RecalculateEarnings;
using Config = SFA.DAS.EmployerIncentives.Infrastructure.Configuration;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services
{
    public class TestLegalEntitiesFunctions : IDisposable
    {
        private readonly TestContext _testContext;
        private readonly TestEmployerIncentivesApi _testEmployerIncentivesApi;
        private readonly Dictionary<string, string> _appConfig;
        private readonly Dictionary<string, string> _hostConfig;
        private readonly TestMessageBus _testMessageBus;
        private readonly List<IHook> _messageHooks;
        private IHost host;
        private bool isDisposed;
        public HandleRefreshLegalEntitiesRequest HttpTriggerRefreshLegalEntities { get; set; }
        public RefreshVendorRegistrationCaseStatus TimerTriggerRefreshVendorRegistrationCaseStatus { get; set; }
        public HandleEarningsResilienceCheck TimerTriggerEarningsResilienceCheck { get; set; }
        public HandleUpdateCollectionCalendarPeriod HttpTriggerUpdateCollectionCalendarPeriod { get; set; }
        public HandleWithdrawalRequest HttpTriggerHandleWithdrawal { get; set; }
        public HandleReinstateApplicationRequest HttpTriggerHandleReinstateApplication { get; set; }
        public HandlePausePaymentsRequest HttpTriggerHandlePausePayments { get; set; }
        public HandleValidationOverrideRequest HttpTriggerHandleValidationOverride { get; set; }

        public HandleBankDetailsRepeatReminderEmails TimerTriggerBankDetailsRepeatReminderEmails { get; set; }
        public HandleRefreshEmploymentCheckRequest HttpTriggerHandleRefreshEmploymentCheck { get; set; }
        public HandleRecalculateEarningsRequest HttpTriggerHandleRecalculateEarningsRequest { get; set; }
        public HandleRevertPaymentsRequest HttpTriggerHandleRevertPaymentsRequest { get; set; }
        public IVrfCaseRefreshRepository VrfCaseRefreshRepository { get; private set; }
        
        public HandleBlockPaymentsRequest HttpTriggerHandleBlockPaymentsRequest { get; set; }

        public HandleReinstatePaymentsRequest HttpTriggerHandleReinstatePaymentsRequest { get; set; }
        public TestLegalEntitiesFunctions(TestContext testContext)
        {
            _testContext = testContext;
            _testEmployerIncentivesApi = testContext.EmployerIncentivesApi;
            _testMessageBus = testContext.TestMessageBus;
            _messageHooks = testContext.Hooks;

            _hostConfig = new Dictionary<string, string>();
            _appConfig = new Dictionary<string, string>
            {
                { "EnvironmentName", "LOCAL_ACCEPTANCE_TESTS" },
                { "ConfigurationStorageConnectionString", "UseDevelopmentStorage=true" },
                { "ConfigNames", "SFA.DAS.EmployerIncentives.Functions" },
                { "NServiceBusConnectionString", "UseDevelopmentStorage=true" },
                { "AzureWebJobsStorage", "UseDevelopmentStorage=true" },
                { "BankDetailsReminderEmailsCutOffDays", "30" }
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

                VrfCaseRefreshRepository = new TestVrfCaseRefreshRepository();
                s.AddSingleton<IVrfCaseRefreshRepository>(VrfCaseRefreshRepository);

                _ = s.AddNServiceBus(typeof(TestLegalEntitiesFunctions).FullName,
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
            TimerTriggerRefreshVendorRegistrationCaseStatus = new RefreshVendorRegistrationCaseStatus(host.Services.GetService(typeof(IVrfCaseRefreshService)) as IVrfCaseRefreshService);
            TimerTriggerEarningsResilienceCheck = new HandleEarningsResilienceCheck(host.Services.GetService(typeof(IEarningsResilienceCheckService)) as IEarningsResilienceCheckService);
            TimerTriggerBankDetailsRepeatReminderEmails = new HandleBankDetailsRepeatReminderEmails(host.Services.GetService(typeof(IEmailService)) as IEmailService, host.Services.GetService(typeof(IConfiguration)) as IConfiguration);
            HttpTriggerUpdateCollectionCalendarPeriod = new HandleUpdateCollectionCalendarPeriod(host.Services.GetService(typeof(ICollectionCalendarService)) as ICollectionCalendarService);
            HttpTriggerHandleWithdrawal = new HandleWithdrawalRequest(host.Services.GetService(typeof(IWithdrawalService)) as IWithdrawalService);
            HttpTriggerHandlePausePayments = new HandlePausePaymentsRequest(host.Services.GetService(typeof(IPaymentsService)) as IPaymentsService);
            HttpTriggerHandleRefreshEmploymentCheck = new HandleRefreshEmploymentCheckRequest(host.Services.GetService(typeof(IEmploymentCheckService)) as IEmploymentCheckService);
            HttpTriggerHandleBlockPaymentsRequest = new HandleBlockPaymentsRequest(host.Services.GetService(typeof(IBlockPaymentsService)) as IBlockPaymentsService);
            HttpTriggerHandleRecalculateEarningsRequest = new HandleRecalculateEarningsRequest(host.Services.GetService(typeof(IRecalculateEarningsService)) as IRecalculateEarningsService);
            HttpTriggerHandleReinstateApplication = new HandleReinstateApplicationRequest(host.Services.GetService(typeof(IWithdrawalService)) as IWithdrawalService);
            HttpTriggerHandleValidationOverride = new HandleValidationOverrideRequest(host.Services.GetService(typeof(IValidationOverrideService)) as IValidationOverrideService);
            HttpTriggerHandleRevertPaymentsRequest = new HandleRevertPaymentsRequest(host.Services.GetService(typeof(IPaymentsService)) as IPaymentsService);
            HttpTriggerHandleReinstatePaymentsRequest = new HandleReinstatePaymentsRequest(host.Services.GetService(typeof(IPaymentsService)) as IPaymentsService);
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

            if (disposing)
            {
                host?.StopAsync();
            }
            host?.Dispose();

            host?.Dispose();

            isDisposed = true;
        }
    }
}
