using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments
{
    public class ManualPaymentsService : IManualPaymentsService
    {
        private readonly IConfiguration _configuration;
        private readonly IBlobContainerFactory _blobContainerFactory;

        public ManualPaymentsService(IConfiguration configuration, IBlobContainerFactory blobContainerFactory)
        {
            _configuration = configuration;
            _blobContainerFactory = blobContainerFactory;
        }

        public Task<List<PaymentRecord>> LoadPaymentRecords(Stream blobStream)
        {
            var cultureInfo = new CultureInfo("en-GB");
            using (var reader = new StreamReader(blobStream))
            {
                var paymentRecords = new List<PaymentRecord>();

                using (var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture))
                {
                    while (csvReader.Read())
                    {
                        var record = new PaymentRecord
                        {
                            DocumentType = csvReader.GetField(0),
                            AccountNumber = csvReader.GetField(1),
                            FundingTypeCode = csvReader.GetField(2),
                            Values = Convert.ToDecimal(csvReader.GetField(3)),
                            PostingDate = Convert.ToDateTime(csvReader.GetField(4), cultureInfo),
                            PaymentDate = Convert.ToDateTime(csvReader.GetField(5), cultureInfo),
                            GLAccountCode = Convert.ToInt64(csvReader.GetField(6)),
                            ExtRef4 = csvReader.GetField(7),
                            CostCentreCodeDimension2 = csvReader.GetField(8),
                            ExtRef3Submitter = csvReader.GetField(9),
                            RemittanceDescription = csvReader.GetField(10)
                        };
                        paymentRecords.Add(record);
                    }
                }

                return Task.FromResult(paymentRecords);
            }
        }

        public async Task<ExcelPackage> LoadManualPaymentsExcelTemplate()
        {
            var container = _blobContainerFactory.GetBlobContainerClient(_configuration["ConfigurationStorageConnectionString"], _configuration["ManualTemplateBlobContainerName"]);

            var blob = container.GetBlobClient(_configuration["ManualTemplateExcelPath"]);

            var fileStream = new MemoryStream();
            await blob.DownloadToAsync(fileStream);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            return await Task.FromResult(new ExcelPackage(fileStream));
        }
        
        public Task<ExcelPackage> AddManualPaymentsToTemplate(ExcelPackage excelPackage, List<PaymentRecord> paymentRecords)
        {
            var templateSheet = excelPackage.Workbook.Worksheets["Template"];

            var row = 10;

            foreach (var paymentRecord in paymentRecords)
            {
                templateSheet.Cells[$"C{row}"].Value = paymentRecord.DocumentType;
                templateSheet.Cells[$"D{row}"].Value = paymentRecord.AccountNumber;
                templateSheet.Cells[$"I{row}"].Value = paymentRecord.FundingTypeCode;
                templateSheet.Cells[$"J{row}"].Value = paymentRecord.Values;
                templateSheet.Cells[$"K{row}"].Value = paymentRecord.PostingDate.ToString("dd/MM/yyyy");
                templateSheet.Cells[$"P{row}"].Value = paymentRecord.PaymentDate.ToString("dd/MM/yyyy");
                templateSheet.Cells[$"Q{row}"].Value = paymentRecord.GLAccountCode;
                templateSheet.Cells[$"T{row}"].Value = paymentRecord.ExtRef4;
                templateSheet.Cells[$"U{row}"].Value = paymentRecord.CostCentreCodeDimension2;
                templateSheet.Cells[$"Z{row}"].Value = paymentRecord.ExtRef3Submitter;
                templateSheet.Cells[$"AB{row}"].Value = paymentRecord.RemittanceDescription;

                row++;
            }
            return Task.FromResult(excelPackage);
        }
        
        public async Task UploadManualPaymentsSpreadsheet(ExcelPackage updatedSpreadsheet)
        {
            var container = _blobContainerFactory.GetBlobContainerClient(_configuration["ConfigurationStorageConnectionString"], _configuration["ManualTemplateBlobContainerName"]);

            var outputPath = string.Format(_configuration["ManualTemplateOutputPath"], DateTime.Now.ToString("yyyy-MM-dd_HHmmss_fff"));
            var spreadsheetBlob = container.GetBlobClient(outputPath);
            var stream = new MemoryStream();
            await updatedSpreadsheet.SaveAsAsync(stream);
            stream.Position = 0;

            await spreadsheetBlob.UploadAsync(stream);
        }
    }
}
