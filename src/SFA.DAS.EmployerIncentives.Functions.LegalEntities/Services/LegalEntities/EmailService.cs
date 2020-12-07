using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _client;

        public EmailService(HttpClient client)
        {
            _client = client;
        }

        public Task SendRepeatReminderEmail(long accountId, long accountLegalEntityId, string emailAddress, string addBankDetailsUrl)
        {
            throw new NotImplementedException();
        }
    }
}
