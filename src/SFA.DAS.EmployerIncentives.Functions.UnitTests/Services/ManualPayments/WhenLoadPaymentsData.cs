using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.ManualPayments
{
    [TestFixture]
    public class WhenLoadPaymentsData
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
            _sut = new ManualPaymentsService(_configuration.Object, _blobContainerFactory.Object);
            _fixture = new Fixture();
        }

        [Test]
        public async Task Then_the_comma_separated_fields_are_loaded_into_payment_records()
        {
            // Arrange
            var paymentRecordLine1 = _fixture.CreateMany<string>(11).ToArray();
            paymentRecordLine1[3] = "123.45";
            paymentRecordLine1[4] = "11/10/2020 00:00:00";
            paymentRecordLine1[5] = "12/11/2020 00:00:00";
            paymentRecordLine1[6] = "1234567";

            var paymentRecordLine2 = _fixture.CreateMany<string>(11).ToArray();
            paymentRecordLine2[3] = "1121.25";
            paymentRecordLine2[4] = "11/11/2020 00:00:00";
            paymentRecordLine2[5] = "05/12/2020 00:00:00";
            paymentRecordLine2[6] = "1456798";

            var paymentList1Csv = string.Join(',', paymentRecordLine1);
            var paymentList2Csv = string.Join(',', paymentRecordLine2);

            var stream = new MemoryStream();
            stream.Write(Encoding.UTF8.GetBytes(paymentList1Csv));
            stream.Write(new byte[] {13, 10});
            stream.Write(Encoding.UTF8.GetBytes(paymentList2Csv));
            stream.Write(new byte[] { 13, 10 });
            stream.Flush();
            stream.Position = 0;

            // Act
            var records = await _sut.LoadPaymentRecords(stream);

            // Assert
            records.Count.Should().Be(2);
            records[0].DocumentType.Should().Be(paymentRecordLine1[0]);
            records[0].AccountNumber.Should().Be(paymentRecordLine1[1]); 
            records[0].FundingTypeCode.Should().Be(paymentRecordLine1[2]);
            records[0].Values.ToString().Should().Be(paymentRecordLine1[3]);
            records[0].PostingDate.ToString("dd/MM/yyyy HH:mm:ss").Should().Be(paymentRecordLine1[4]);
            records[0].PaymentDate.ToString("dd/MM/yyyy HH:mm:ss").Should().Be(paymentRecordLine1[5]);
            records[0].GLAccountCode.ToString().Should().Be(paymentRecordLine1[6]);
            records[0].ExtRef4.Should().Be(paymentRecordLine1[7]);
            records[0].CostCentreCodeDimension2.Should().Be(paymentRecordLine1[8]);
            records[0].ExtRef3Submitter.Should().Be(paymentRecordLine1[9]);
            records[0].RemittanceDescription.Should().Be(paymentRecordLine1[10]);
            records[1].DocumentType.Should().Be(paymentRecordLine2[0]);
            records[1].AccountNumber.Should().Be(paymentRecordLine2[1]);
            records[1].FundingTypeCode.Should().Be(paymentRecordLine2[2]);
            records[1].Values.ToString().Should().Be(paymentRecordLine2[3]);
            records[1].PostingDate.ToString().Should().Be(paymentRecordLine2[4]);
            records[1].PaymentDate.ToString().Should().Be(paymentRecordLine2[5]);
            records[1].GLAccountCode.ToString().Should().Be(paymentRecordLine2[6]);
            records[1].ExtRef4.Should().Be(paymentRecordLine2[7]);
            records[1].CostCentreCodeDimension2.Should().Be(paymentRecordLine2[8]);
            records[1].ExtRef3Submitter.Should().Be(paymentRecordLine2[9]);
            records[1].RemittanceDescription.Should().Be(paymentRecordLine2[10]);
        }

    }
}
