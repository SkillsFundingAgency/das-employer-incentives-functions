using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleManualTemplateFileUpload
    {
        private readonly IManualPaymentsService _manualPaymentsService;
        private const int PaymentRecordsLimit = 2500;

        public HandleManualTemplateFileUpload(IManualPaymentsService manualPaymentsService)
        {
            _manualPaymentsService = manualPaymentsService;
        }

        [FunctionName("HandleManualTemplateFileUpload")]
        public async Task Run([BlobTrigger("employer-incentives-eups/PaymentData/{name}", Connection = "ConfigurationStorageConnectionString")]Stream blobStream, string name, ILogger log)
        {
            var paymentRecords = await _manualPaymentsService.LoadPaymentRecords(blobStream);

            while (paymentRecords.Count > 0)
            {
                var excelTemplate = await _manualPaymentsService.LoadManualPaymentsExcelTemplate();

                var paymentRecordsBatch = paymentRecords.Take(PaymentRecordsLimit).ToList();
                
                await _manualPaymentsService.AddManualPaymentsToTemplate(excelTemplate, paymentRecordsBatch);

                await _manualPaymentsService.UploadManualPaymentsSpreadsheet(excelTemplate);

                paymentRecords.RemoveRange(0, paymentRecordsBatch.Count);
            }

        }
    }
}
