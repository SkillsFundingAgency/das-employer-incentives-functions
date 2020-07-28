using System;
using WireMock.Server;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services
{
    public class TestEmployerIncentivesApi : IDisposable
    {
        private bool isDisposed;

        public string BaseAddress { get; private set; }

        public WireMockServer MockServer { get; private set; }

        public TestEmployerIncentivesApi()
        {
            MockServer = WireMockServer.Start(ssl: true);
            BaseAddress = MockServer.Urls[0];
            BaseAddress += "/api";
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                if (MockServer.IsStarted)
                {
                    MockServer.Stop();
                }
                MockServer.Dispose();
            }

            isDisposed = true;
        }
    }
}
