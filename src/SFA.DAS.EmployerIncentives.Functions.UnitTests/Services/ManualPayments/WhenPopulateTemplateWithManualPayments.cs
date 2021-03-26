using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OfficeOpenXml;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.ManualPayments
{
    [TestFixture]
    public class WhenPopulateTemplateWithManualPayments
    {
        private Mock<IConfiguration> _configuration;
        private Mock<IBlobContainerFactory> _blobContainerFactory;
        private ManualPaymentsService _sut;
        private Fixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _configuration = new Mock<IConfiguration>();
            _blobContainerFactory = new Mock<IBlobContainerFactory>();
            _fixture = new Fixture();
            _sut = new ManualPaymentsService(_configuration.Object, _blobContainerFactory.Object);
        }

        [Test]
        public async Task Then_the_spreadsheet_is_populated_with_values_from_the_payment_records()
        {
            // Arrange
            var excelTemplateRawData = Resource1.TestEUPSTemplate;
            var excelTemplateStream = new MemoryStream();
            excelTemplateStream.Write(excelTemplateRawData);
            excelTemplateStream.Flush();
            excelTemplateStream.Position = 0;

            var excelPackage = new ExcelPackage(excelTemplateStream);

            var paymentRecords = _fixture.CreateMany<PaymentRecord>(10).ToList();

            // Act
            var updatedPackage = await _sut.AddManualPaymentsToTemplate(excelPackage, paymentRecords);

            // Assert
            var templateSheet = updatedPackage.Workbook.Worksheets["Template"];
            for(var index = 0; index < paymentRecords.Count; index++)
            {
                var rowNumber = 10 + index;
                templateSheet.Cells[$"C{rowNumber}"].Value.Should().Be(paymentRecords[index].DocumentType);
                templateSheet.Cells[$"D{rowNumber}"].Value.Should().Be(paymentRecords[index].AccountNumber);
                templateSheet.Cells[$"I{rowNumber}"].Value.Should().Be(paymentRecords[index].FundingTypeCode);
                templateSheet.Cells[$"J{rowNumber}"].Value.Should().Be(paymentRecords[index].Values);
                templateSheet.Cells[$"K{rowNumber}"].Value.Should().Be(paymentRecords[index].PostingDate.ToString("dd/MM/yyyy"));
                templateSheet.Cells[$"P{rowNumber}"].Value.Should().Be(paymentRecords[index].PaymentDate.ToString("dd/MM/yyyy"));
                templateSheet.Cells[$"Q{rowNumber}"].Value.Should().Be(paymentRecords[index].GLAccountCode);
                templateSheet.Cells[$"T{rowNumber}"].Value.Should().Be(paymentRecords[index].ExtRef4);
                templateSheet.Cells[$"U{rowNumber}"].Value.Should().Be(paymentRecords[index].CostCentreCodeDimension2);
                templateSheet.Cells[$"Z{rowNumber}"].Value.Should().Be(paymentRecords[index].ExtRef3Submitter);
                templateSheet.Cells[$"AB{rowNumber}"].Value.Should().Be(paymentRecords[index].RemittanceDescription);
            }
        }
    }
}
