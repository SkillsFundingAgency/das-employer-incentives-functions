using Microsoft.Azure.WebJobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Commands;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleAddEmployerVendorIdCommand
    {
        private readonly IEmployerVendorIdService _addEmployerVendorIdService;

        public HandleAddEmployerVendorIdCommand(IEmployerVendorIdService addEmployerVendorIdService)
        {
            _addEmployerVendorIdService = addEmployerVendorIdService;
        }

        [FunctionName("HandleAddEmployerVendorIdCommand")]
        public Task RunEvent([NServiceBusTrigger(Endpoint = QueueNames.AddEmployerVendorId)] AddEmployerVendorIdCommand command)
        {
            return _addEmployerVendorIdService.GetAndAddEmployerVendorId(command.HashedLegalEntityId);
        }
    }
}
