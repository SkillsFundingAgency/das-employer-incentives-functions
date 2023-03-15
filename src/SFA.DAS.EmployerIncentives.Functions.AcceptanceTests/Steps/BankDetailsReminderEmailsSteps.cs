using FluentAssertions;
using HandlebarsDotNet;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services;
using System.IO;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "BankDetailsReminderEmails")]
    public class BankDetailsReminderEmailsSteps : StepsBase
    {
        private readonly TestContext _testContext;

        public BankDetailsReminderEmailsSteps(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
        }

        [When(@"a bank details reminder emails job is triggered")]
        public Task WhenABankDetailsReminderEmailsJobIsTriggered()
        {
            var env = Environment.GetEnvironmentVariable("EnvironmentName");
            var configFileName = "NLog.config";
            if (string.IsNullOrEmpty(env) || env.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
            {
                configFileName = "NLog.local.config";
            }

            var rootDirectory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.Parent;
            var configFilePath = Directory.GetFiles(rootDirectory.FullName, "NLog.config", SearchOption.AllDirectories);

            configFilePath.Count().Should().Be(2);

            return Task.CompletedTask;

            //var rootDirectory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.Parent;

            //try
            //{   
                
            //    await _testContext.LegalEntitiesFunctions.Start();
            //}
            //catch(System.Exception ex)
            //{
            //    ex.StackTrace.Should().Be("Empty");
            //}

            //_testContext.EmployerIncentivesApi.MockServer
            //    .Given(
            //        Request
            //            .Create()
            //            .WithPath(x => x.Contains("email/bank-details-repeat-reminders"))
            //            .UsingPost())
            //    .RespondWith(
            //        Response.Create(new ResponseMessage())
            //            .WithStatusCode(HttpStatusCode.OK));

            //await _testContext.LegalEntitiesFunctions.TimerTriggerBankDetailsRepeatReminderEmails.RunTimer(null, new TestLogger());
        }

        [Then(@"the Employer Incentives API is called to check for applications where the account has no bank details")]
        public void ThenTheEmployerIncentivesAPIIsCalledToCheckForApplicationsWhereTheAccountHasNoBankDetails()
        {
            //var requests = _testContext
            //    .EmployerIncentivesApi
            //    .MockServer
            //    .FindLogEntries(
            //        Request
            //            .Create()
            //            .WithPath(x => x.Contains("email/bank-details-repeat-reminders"))
            //            .UsingPost()).AsEnumerable();

            //requests.Should().HaveCount(1, "Expected request to APIM was not found in Mock server logs");
        }

    }
}
