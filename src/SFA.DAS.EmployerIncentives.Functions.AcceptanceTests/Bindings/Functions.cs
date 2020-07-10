using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Config = SFA.DAS.EmployerIncentives.Infrastructure.Configuration;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Bindings
{
    [Binding]
    public class Functions
    {
        private readonly TestContext _context;

        public Functions(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario()]
        public async Task InitialiseFunctions()
        {
            var startUp = new Startup();

            var config = new Dictionary<string, string>
            {
                { "EnvironmentName", "LOCAL" },
                { "ConfigurationStorageConnectionString", "UseDevelopmentStorage=true" },
                { "ConfigNames", "SFA.DAS.EmployerIncentives.Functions" }
            };

            var host = new HostBuilder()
                .ConfigureHostConfiguration(a =>
                {
                    a.AddInMemoryCollection(config);
                })
               .ConfigureWebJobs(startUp.Configure);

            _ = host.ConfigureServices((s) =>
            {
                s.Configure<Config.EmployerIncentivesApi>(a =>
                {
                    a.ApiBaseUrl = _context.EmployerIncentivesApi.BaseAddress;
                    a.ClientId = "";
                });

                s.AddNServiceBus(new LoggerFactory().CreateLogger<Functions>(), 
                    (o) =>
                    {
                        o.EndpointConfiguration = (endpoint) =>
                        {
                            endpoint.UseTransport<LearningTransport>().StorageDirectory(Path.Combine(_context.TestDirectory.FullName, ".learningtransport"));
                            return endpoint;
                        };
                        o.OnMessageReceived = (message) =>
                        {
                            _context.FunctionHooks?.OnMessageReceived(message);
                        };
                    });
            });

            host.UseEnvironment("LOCAL");

            _context.FunctionsHost = await host.StartAsync();
        }
    }

    public class TriggeredFunctionExecutorHook : ITriggeredFunctionExecutor
    {
        private readonly ITriggeredFunctionExecutor _triggeredFunctionExecutor;

        public TriggeredFunctionExecutorHook(ITriggeredFunctionExecutor triggeredFunctionExecutor)
        {
            _triggeredFunctionExecutor = triggeredFunctionExecutor;
        }
        public Task<FunctionResult> TryExecuteAsync(TriggeredFunctionData input, CancellationToken cancellationToken)
        {
            return _triggeredFunctionExecutor.TryExecuteAsync(input, cancellationToken);
        }
    }

}
