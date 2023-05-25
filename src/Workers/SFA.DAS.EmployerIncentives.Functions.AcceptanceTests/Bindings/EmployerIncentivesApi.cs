using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services;
using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "employerIncentivesApi")]
    public class EmployerIncentivesApi
    {
        private readonly TestContext _context;

        public EmployerIncentivesApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario()]
        public void InitialiseApi()
        {
            _context.EmployerIncentivesApi = new TestEmployerIncentivesApi();
        }
    }
}
