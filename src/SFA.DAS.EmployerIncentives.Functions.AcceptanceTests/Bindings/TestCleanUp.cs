using System.IO;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Bindings
{
    [Binding]
    public class TestCleanUp
    {
        private readonly TestContext _context;
        public TestCleanUp(TestContext context)
        {
            _context = context;
        }

        [AfterScenario(Order = 100)]
        public async Task CleanUp()
        {
            try
            {
                if (_context.TestMessageBus.IsRunning)
                {
                    await _context.TestMessageBus.Stop();
                }
                _context.LegalEntitiesFunctions?.Dispose();
                _context.EmployerIncentivesApi?.Dispose();
                Directory.Delete(_context.TestDirectory.FullName, true);
            }
            catch { }
        }
    }
}
