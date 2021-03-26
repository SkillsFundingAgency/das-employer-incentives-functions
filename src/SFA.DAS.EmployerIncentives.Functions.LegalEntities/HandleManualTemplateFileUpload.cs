using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleManualTemplateFileUpload
    {
        private readonly IManualPaymentsService _manualPaymentsService;
        public HandleManualTemplateFileUpload(IManualPaymentsService manualPaymentsService)
        {
            _manualPaymentsService = manualPaymentsService;
        }

        [FunctionName("HandleManualTemplateFileUpload")]
        public async Task Run([BlobTrigger("employer-incentives-eups/PaymentData/{name}", Connection = "ConfigurationStorageConnectionString")]Stream blobStream, string name, ILogger log)
        {
            var paymentRecords = await _manualPaymentsService.LoadPaymentRecords(blobStream);

            var excelTemplate = await _manualPaymentsService.LoadManualPaymentsExcelTemplate();

            var updatedSpreadsheet = await _manualPaymentsService.AddManualPaymentsToTemplate(excelTemplate, paymentRecords);

            await _manualPaymentsService.UploadManualPaymentsSpreadsheet(updatedSpreadsheet);
        }
    }
}
