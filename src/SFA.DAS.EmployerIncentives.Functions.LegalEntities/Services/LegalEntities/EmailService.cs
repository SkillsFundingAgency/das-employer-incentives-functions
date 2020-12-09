using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System;
using System.Net.Http;
using System.Text;
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

        public async Task SendRepeatReminderEmails(DateTime applicationCutOffDate)
        {
            var url = "email/bank-details-repeat-reminders";
            var request = new BankDetailRepeatReminderEmailsRequest { ApplicationCutOffDate = applicationCutOffDate };
            var response = await _client.PostAsync(url, request.GetStringContent());
            response.EnsureSuccessStatusCode();
        }
    }
}
