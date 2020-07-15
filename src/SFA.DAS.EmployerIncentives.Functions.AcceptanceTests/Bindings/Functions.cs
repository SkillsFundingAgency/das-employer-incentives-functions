using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Bindings
{
    [Binding]
    public class Functions
    {
        private readonly TestContext _context;
        private readonly Dictionary<string, string> _hostConfig;
        private readonly Dictionary<string, string> _appConfig;

        public Functions(TestContext context)
        {
            _context = context;
            _hostConfig = new Dictionary<string, string>();
            _appConfig = new Dictionary<string, string>
            {
                { "EnvironmentName", "LOCAL" },
                { "ConfigurationStorageConnectionString", "UseDevelopmentStorage=true" },
                { "ConfigNames", "SFA.DAS.EmployerIncentives.Functions" }
            };
        }

        [BeforeScenario()]
        public async Task InitialiseFunctions()
        {
            _context.LegalEntitiesFunctions = new TestLegalEntitiesFunctions(
                _context.EmployerIncentivesApi,
                _context.TestMessageBus,
                _context.Hooks
                );
            await _context.LegalEntitiesFunctions.Start();
       }
    }
}