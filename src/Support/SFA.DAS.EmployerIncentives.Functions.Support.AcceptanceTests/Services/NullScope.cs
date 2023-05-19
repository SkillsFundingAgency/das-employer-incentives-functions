using System;

namespace SFA.DAS.EmployerIncentives.Functions.Support.AcceptanceTests.Services
{
    public class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        private NullScope() { }

        public void Dispose() { }
    }
}
