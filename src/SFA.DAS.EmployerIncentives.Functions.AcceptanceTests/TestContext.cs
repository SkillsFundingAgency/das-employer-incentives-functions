using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Hooks;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests
{
    public class TestContext
    {
        public DirectoryInfo TestDirectory { get; set; }
        public TestMessageBus TestMessageBus { get; set; }
        public TestEmployerIncentivesApi EmployerIncentivesApi { get; set; }
        public TestLegalEntitiesFunctions LegalEntitiesFunctions { get; set; }
        public TestData TestData { get; set; }
        public List<IHook> Hooks { get; set; }

        public TestContext()
        {
            TestDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString()));
            if (!TestDirectory.Exists)
            {
                Directory.CreateDirectory(TestDirectory.FullName);
            }
            TestData = new TestData();
            Hooks = new List<IHook>();
        }
    }    
}


