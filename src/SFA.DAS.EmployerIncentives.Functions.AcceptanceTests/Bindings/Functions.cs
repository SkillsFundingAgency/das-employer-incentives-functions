using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

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

       // [BeforeScenario()]
       // public async Task InitialiseFunctions()
       // {
       //     _context.LegalEntitiesFunctions = new TestLegalEntitiesFunctions(_context);
       //     await _context.LegalEntitiesFunctions.Start();
       //}
    }
}