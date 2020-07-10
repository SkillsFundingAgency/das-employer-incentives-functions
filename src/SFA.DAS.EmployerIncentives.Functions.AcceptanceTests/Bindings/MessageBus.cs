using System.IO;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "messageBus")]
    public class MessageBus
    {
        private readonly TestContext _context;

        public MessageBus(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario()]
        public Task InitialiseMessageBus()
        {
            _context.TestMessageBus = new TestMessageBus();
            return _context.TestMessageBus.Start(new DirectoryInfo(Path.Combine(_context.TestDirectory.FullName, ".learningtransport")));
        }
    }
}
