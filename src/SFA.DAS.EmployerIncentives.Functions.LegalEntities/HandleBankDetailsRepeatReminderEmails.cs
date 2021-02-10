using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleBankDetailsRepeatReminderEmails
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private const int DefaultApplicationCutOffDays = 30;

        public HandleBankDetailsRepeatReminderEmails(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        [FunctionName("TimerBankDetailsReminderEmails")]
        public async Task RunTimer([TimerTrigger("%BankDetailsReminderEmailsTriggerTime%", RunOnStartup = false)]TimerInfo myTimer, ILogger log)
        {
            await RunBankDetailsRepeatReminderEmails(log);
        }

        [FunctionName("HttpBankDetailsReminderEmails")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function)] HttpRequest request, ILogger log)
        {
            await RunBankDetailsRepeatReminderEmails(log);

            return new OkResult();
        }

        private async Task RunBankDetailsRepeatReminderEmails(ILogger log)
        {
            log.LogInformation("Started bank details repeat reminder emails");
            var cutOffDays = Convert.ToInt32(_configuration["BankDetailsReminderEmailsCutOffDays"]);
            if (cutOffDays == 0)
            {
                cutOffDays = DefaultApplicationCutOffDays;
            }
            var cutoffDate = DateTime.Today.AddDays(-1 * cutOffDays);
            await _emailService.SendRepeatReminderEmails(cutoffDate);
            log.LogInformation("Completed bank details repeat reminder emails");
        }
    }
}
