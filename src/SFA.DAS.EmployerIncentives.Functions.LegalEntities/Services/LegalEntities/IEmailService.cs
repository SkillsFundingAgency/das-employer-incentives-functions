using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public interface IEmailService
    {
        Task SendRepeatReminderEmail(long accountId, long accountLegalEntityId, string emailAddress, string addBankDetailsUrl);
    }
}
