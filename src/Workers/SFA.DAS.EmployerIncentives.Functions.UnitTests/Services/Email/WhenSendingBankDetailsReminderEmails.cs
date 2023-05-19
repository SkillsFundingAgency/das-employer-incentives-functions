using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.Email
{
    public class WhenSendingBankDetailsReminderEmails
    {
        private EmailService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;

        [SetUp]
        public void Setup()
        {
            _baseAddress = new Uri("http://localhost");
            _testClient = new TestHttpClient(_baseAddress);
            _testClient.SetUpPostAsAsync(HttpStatusCode.OK);

            _sut = new EmailService(_testClient);
        }

        [Test]
        public async Task Then_API_is_invoked()
        {
            await _sut.SendRepeatReminderEmails(DateTime.Today);

            _testClient.VerifyPostAsAsync("email/bank-details-repeat-reminders", Times.Once());
        }
    }
}
