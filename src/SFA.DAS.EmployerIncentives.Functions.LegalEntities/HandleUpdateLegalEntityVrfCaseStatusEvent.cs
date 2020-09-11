using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Messages.Events;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleUpdateLegalEntityVrfCaseStatusEvent
    {
        private readonly IVendorRegistrationFormService _vendorRegistrationFormService;

        public HandleUpdateLegalEntityVrfCaseStatusEvent(IVendorRegistrationFormService vendorRegistrationFormService)
        {
            _vendorRegistrationFormService = vendorRegistrationFormService;
        }

        [FunctionName("HandleGetLegalEntityVrfCaseDetailsEvent")]
        public Task RunEvent([NServiceBusTrigger(Endpoint = QueueNames.UpdateLegalEntityVrfCaseStatusEvent)] UpdateLegalEntityVrfCaseStatusEvent message)
        {
            return _vendorRegistrationFormService.UpdateVrfCaseStatus(message.LegalEntityId, message.VrfCaseId);
        }
    }
}
