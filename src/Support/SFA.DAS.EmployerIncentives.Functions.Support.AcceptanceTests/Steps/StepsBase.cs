using NUnit.Framework;
using TestContext = SFA.DAS.EmployerIncentives.Functions.Support.AcceptanceTests.TestContext;

[assembly: Parallelizable(ParallelScope.Fixtures)]
namespace SFA.DAS.EmployerIncentives.Functions.Support.AcceptanceTests.Steps
{
    public class StepsBase
    {
        protected readonly TestContext TestContext;

        public StepsBase(TestContext testContext)
        {
            TestContext = testContext;
        }
    }
}
