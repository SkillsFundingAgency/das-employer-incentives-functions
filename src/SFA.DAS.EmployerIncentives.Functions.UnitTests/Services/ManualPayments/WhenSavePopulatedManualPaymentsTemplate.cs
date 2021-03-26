using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OfficeOpenXml;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.ManualPayments
{
    [TestFixture]
    public class WhenSavePopulatedManualPaymentsTemplate
    {
        private Mock<IConfiguration> _configuration;
        private Mock<IBlobContainerFactory> _blobContainerFactory;
        private ManualPaymentsService _sut;
        private Fixture _fixture;
        private string _configConnectionString;
        private string _blobContainerName;
        private string _outputPath;

        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();
            _configConnectionString = _fixture.Create<string>();
            _blobContainerName = _fixture.Create<string>();
            _outputPath = "Output{0}Path";
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(x => x["ConfigurationStorageConnectionString"]).Returns(_configConnectionString);
            _configuration.Setup(x => x["ManualTemplateBlobContainerName"]).Returns(_blobContainerName);
            _configuration.Setup(x => x["ManualTemplateOutputPath"]).Returns(_outputPath);
            _blobContainerFactory = new Mock<IBlobContainerFactory>();

            _sut = new ManualPaymentsService(_configuration.Object, _blobContainerFactory.Object);
        }

        [Test]
        public async Task Then_the_updated_spreadsheet_is_written_to_blob_storage()
        {
            var blobContainerClient = new Mock<BlobContainerClient>();
            var blobClient = new Mock<BlobClient>();
            blobClient.Setup(x => x.DownloadToAsync(It.IsAny<MemoryStream>()));
            blobContainerClient.Setup(x => x.GetBlobClient(It.Is<string>(x => x.StartsWith("Output")))).Returns(blobClient.Object);
            _blobContainerFactory.Setup(x => x.GetBlobContainerClient(_configConnectionString, _blobContainerName))
                .Returns(blobContainerClient.Object);
            
            var excelTemplateRawData = Resource1.TestEUPSTemplate;
            var excelTemplateStream = new MemoryStream();
            excelTemplateStream.Write(excelTemplateRawData);
            excelTemplateStream.Flush();
            excelTemplateStream.Position = 0;

            var excelPackage = new ExcelPackage(excelTemplateStream);
            await _sut.UploadManualPaymentsSpreadsheet(excelPackage);

            blobClient.Verify(x => x.UploadAsync(It.IsAny<MemoryStream>()), Times.Once);
        }
    }
}
