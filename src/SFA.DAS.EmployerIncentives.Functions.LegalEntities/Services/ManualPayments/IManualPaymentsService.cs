using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OfficeOpenXml;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments
{
    public interface IManualPaymentsService
    {
        Task<List<PaymentRecord>> LoadPaymentRecords(Stream blobStream);
        Task<ExcelPackage> LoadManualPaymentsExcelTemplate();
        Task<ExcelPackage> AddManualPaymentsToTemplate(ExcelPackage excelPackage, List<PaymentRecord> paymentRecords);
        Task UploadManualPaymentsSpreadsheet(ExcelPackage updatedSpreadsheet);
    }
}
