using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments;
using AutoFixture;
using Azure.Storage.Blobs;
using NUnit.Framework;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.ManualPayments
{
    [TestFixture]
    public class WhenLoadPaymentsTemplate
    {
        private Mock<IConfiguration> _configuration;
        private Mock<IBlobContainerFactory> _blobContainerFactory;
        private ManualPaymentsService _sut;
        private Fixture _fixture;
        private string _configConnectionString;
        private string _blobContainerName;
        private string _manualTemplatePath;

        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();
            _configConnectionString = _fixture.Create<string>();
            _blobContainerName = _fixture.Create<string>();
            _manualTemplatePath = _fixture.Create<string>();
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(x => x["ConfigurationStorageConnectionString"]).Returns(_configConnectionString);
            _configuration.Setup(x => x["ManualTemplateBlobContainerName"]).Returns(_blobContainerName);
            _configuration.Setup(x => x["ManualTemplateExcelPath"]).Returns(_manualTemplatePath);
            _blobContainerFactory = new Mock<IBlobContainerFactory>();

            _sut = new ManualPaymentsService(_configuration.Object, _blobContainerFactory.Object);
        }

        [Test]
        public async Task Then_the_payments_template_is_retrieved_from_blob_storage()
        {
            var blobContainerClient = new Mock<BlobContainerClient>();
            var blobClient = new Mock<BlobClient>();
            blobClient.Setup(x => x.DownloadToAsync(It.IsAny<MemoryStream>()));
            blobContainerClient.Setup(x => x.GetBlobClient(_manualTemplatePath)).Returns(blobClient.Object);
            _blobContainerFactory.Setup(x => x.GetBlobContainerClient(_configConnectionString, _blobContainerName))
                .Returns(blobContainerClient.Object);

            await _sut.LoadManualPaymentsExcelTemplate();

            blobClient.Verify(x => x.DownloadToAsync(It.IsAny<MemoryStream>()), Times.Once);
        }
    }
}
