using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class EmailServiceWithLogging : IEmailService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailServiceWithLogging> _logger;

        public EmailServiceWithLogging(
            IEmailService emailService,
            ILogger<EmailServiceWithLogging> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task SendRepeatReminderEmails(DateTime applicationCutOffDate)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"Calling IEmailService.SendRepeatReminderEmails");

                await _emailService.SendRepeatReminderEmails(applicationCutOffDate);

                _logger.Log(LogLevel.Information, $"Called IEmailService.SendRepeatReminderEmails");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling IEmailService.SendRepeatReminderEmails");

                throw;
            }
        }
    }
}
