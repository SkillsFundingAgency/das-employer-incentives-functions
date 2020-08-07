using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleSignAgreementEvent
    {
        private readonly IAgreementsService _agreementsService;

        public HandleSignAgreementEvent(IAgreementsService agreementsService)
        {
            _agreementsService = agreementsService;
        }

        [FunctionName("HandleSignAgreementEvent")]
        public Task RunEvent([NServiceBusTrigger(Endpoint = QueueNames.AgreementSigned)] SignedAgreementEvent message)
        {
            var signRequest = new SignAgreementRequest
            {
                AccountId = message.AccountId,
                AccountLegalEntityId = message.AccountLegalEntityId,
                AgreementVersion = message.SignedAgreementVersion
            };
            
            return _agreementsService.SignAgreement(signRequest);
        }
    }
}
