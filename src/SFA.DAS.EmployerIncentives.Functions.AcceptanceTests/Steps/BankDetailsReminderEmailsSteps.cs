using FluentAssertions;
using HandlebarsDotNet;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Services;
using System.Linq;
using System.Net;
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
        public void Task WhenABankDetailsReminderEmailsJobIsTriggered()
        {
            //await _testContext.LegalEntitiesFunctions.Start();
            return Task.CompletedTask;

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
