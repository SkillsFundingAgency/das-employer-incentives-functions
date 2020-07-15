using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.Fixtures)]
namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Steps
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
