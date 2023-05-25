using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.Support.AcceptanceTests.Services;
using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Functions.Support.AcceptanceTests.Bindings
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
            _context.LegalEntitiesFunctions = new TestLegalEntitiesFunctions(_context);
            await _context.LegalEntitiesFunctions.Start();
       }
    }
}