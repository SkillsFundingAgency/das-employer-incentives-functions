using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public interface IEmailService
    {
        Task SendRepeatReminderEmails(DateTime applicationCutOffDate);
    }
}
