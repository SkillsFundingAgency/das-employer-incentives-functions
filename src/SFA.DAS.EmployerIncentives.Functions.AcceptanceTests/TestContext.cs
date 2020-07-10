using FluentAssertions;
using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Hooks;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests
{
    public class TestContext
    {
        public DirectoryInfo TestDirectory { get; set; }
        public TestMessageBus TestMessageBus { get; set; }
        public TestEmployerIncentivesApi EmployerIncentivesApi { get; set; }
        public IHost FunctionsHost { get; set; }
        public TestData TestData { get; set; }
        public FunctionHooks FunctionHooks { get; set; }
        public WaitForResult WaitForResult { get; set; }

        public TestContext()
        {
            TestDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString()));
            if (!TestDirectory.Exists)
            {
                Directory.CreateDirectory(TestDirectory.FullName);
            }
            TestData = new TestData();
        }

        public async Task WaitForMessage(
                    Func<Task> func,
                    bool assertOnTimeout = true,
                    bool assertOnError = false,
                    int timeoutInMs = 15000)
        {
#if DEBUG
            timeoutInMs = 1000;
#endif
            WaitForResult = new WaitForResult();

            FunctionHooks = new FunctionHooks
            {
                OnMessageReceived = (message) => { WaitForResult.SetHasStarted(); },
                OnMessageProcesses = (message) => { WaitForResult.SetHasCompleted(); },
                OnMessageErrored = (ex, message) => { WaitForResult.SetHasErrored(ex); }
            };

            try
            {
                await func();
            }
            catch (Exception ex)
            {
                WaitForResult.SetHasErrored(ex);
            }
            await WaitForHandlerCompletion(WaitForResult, timeoutInMs);

            if (assertOnTimeout)
            {
                WaitForResult.HasTimedOut.Should().Be(false, "handler should not have timed out");
            }

            if (assertOnError)
            {
                WaitForResult.HasErrored.Should().Be(false, $"handler should not have errored with error '{WaitForResult.LastException?.Message}'");
            }
        }
        private bool hasTimedOut = false;
        private async Task WaitForHandlerCompletion(WaitForResult waitForResult, int timeoutInMs)
        {
            using (Timer timer = new Timer(new TimerCallback(TimedOutCallback), null, timeoutInMs, Timeout.Infinite))
            {
                while (!waitForResult.HasCompleted && !waitForResult.HasTimedOut)
                {
                    await Task.Delay(100);
                }
            }
            if (hasTimedOut)
            {
                waitForResult.SetHasTimedOut();
            }
        }

        private void TimedOutCallback(object state)
        {
            hasTimedOut = true;
        }
    }


    public class WaitForResult
    {
        public bool HasTimedOut { get; private set; }
        public bool HasStarted { get; private set; }
        public bool HasErrored { get; private set; }
        public Exception LastException { get; private set; }
        public bool HasCompleted { get; private set; }

        public void SetHasTimedOut()
        {
            HasTimedOut = true;
        }

        public void SetHasCompleted()
        {
            HasCompleted = true;
        }

        public void SetHasStarted()
        {
            HasStarted = true;
        }

        public void SetHasErrored(Exception ex)
        {
            HasErrored = true;
            LastException = ex;
        }

        public WaitForResult()
        {
            HasTimedOut = false;
            HasCompleted = false;
            HasStarted = false;
            HasErrored = false;
            LastException = null;
        }
    }
}


