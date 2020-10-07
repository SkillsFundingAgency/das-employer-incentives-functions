using Moq;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Hooks;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using System;
using System.Collections.Generic;
using System.IO;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests
{
    public class TestContext : IDisposable
    {
        public DirectoryInfo TestDirectory { get; set; }
        public TestMessageBus TestMessageBus { get; set; }
        public TestEmployerIncentivesApi EmployerIncentivesApi { get; set; }
        public TestLegalEntitiesFunctions LegalEntitiesFunctions { get; set; }
        public Mock<IDateTimeProvider> DateTimeProvider { get; set; }
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
            DateTimeProvider = new Mock<IDateTimeProvider>();
        }
        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                EmployerIncentivesApi?.Dispose();
                LegalEntitiesFunctions?.Dispose();
            }

            _isDisposed = true;
        }
    }    
}


