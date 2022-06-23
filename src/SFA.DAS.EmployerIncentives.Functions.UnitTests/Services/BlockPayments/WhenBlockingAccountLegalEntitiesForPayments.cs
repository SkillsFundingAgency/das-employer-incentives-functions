using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments.Types;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.BlockPayments
{
    [TestFixture]
    public class WhenBlockingAccountLegalEntitiesForPayments
    {
        private BlockPaymentsService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _baseAddress = new Uri(@"http://localhost");
            _testClient = new TestHttpClient(_baseAddress);

            _testClient.SetUpPostAsAsync(HttpStatusCode.OK);

            _sut = new BlockPaymentsService(_testClient);
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client()
        {
            // Arrange
            var blockPaymentsRequest = BuildValidBlockPaymentsRequest();

            // Act
            await _sut.BlockAccountLegalEntitiesForPayments(blockPaymentsRequest);

            // Assert
            _testClient.VerifyPatchAsAsync("blockedpayments", Times.Once());
        }

        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.BadRequest)]
        public void Then_a_BlockPaymentServiceException_is_thrown_when_the_client_returns(HttpStatusCode clientResponse)
        {
            // Arrange
            var blockPaymentsRequest = BuildValidBlockPaymentsRequest();
            _testClient.SetUpPatchAsAsync(clientResponse);

            // Act
            Func<Task> result = async () => await _sut.BlockAccountLegalEntitiesForPayments(blockPaymentsRequest);

            // Assert
            result.Should().Throw<BlockPaymentsServiceException>();
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_service_request_is_not_set()
        {
            // Arrange
            var request = BuildValidBlockPaymentsRequest();
            request.ServiceRequest = null;
            var service = new BlockPaymentsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.BlockAccountLegalEntitiesForPayments(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request is not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_service_request_task_id_is_not_set()
        {
            // Arrange
            var request = BuildValidBlockPaymentsRequest();
            request.ServiceRequest.TaskId = null;
            var service = new BlockPaymentsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.BlockAccountLegalEntitiesForPayments(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request Task Id is not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_service_request_decision_reference_is_not_set()
        {
            // Arrange
            var request = BuildValidBlockPaymentsRequest();
            request.ServiceRequest.DecisionReference = null;
            var service = new BlockPaymentsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.BlockAccountLegalEntitiesForPayments(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request Decision Reference is not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_service_request_created_date_is_not_set()
        {
            // Arrange
            var request = BuildValidBlockPaymentsRequest();
            request.ServiceRequest.TaskCreatedDate = null;
            var service = new BlockPaymentsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.BlockAccountLegalEntitiesForPayments(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Service Request Task Created Date is not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_vendor_blocks_are_not_set()
        {
            // Arrange
            var request = BuildValidBlockPaymentsRequest();
            request.VendorBlocks = null;
            var service = new BlockPaymentsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.BlockAccountLegalEntitiesForPayments(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Vendor Blocks are not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_vendor_blocks_are_empty()
        {
            // Arrange
            var request = BuildValidBlockPaymentsRequest();
            request.VendorBlocks = new List<VendorBlock>();
            var service = new BlockPaymentsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.BlockAccountLegalEntitiesForPayments(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Vendor Blocks are not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_vendor_block_vendor_id_is_not_set()
        {
            // Arrange
            var request = BuildValidBlockPaymentsRequest();
            request.VendorBlocks[0].VendorId = null;
            var service = new BlockPaymentsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.BlockAccountLegalEntitiesForPayments(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Vendor Block Vendor Id is not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_vendor_block_end_date_is_not_set()
        {
            // Arrange
            var request = BuildValidBlockPaymentsRequest();
            request.VendorBlocks[0].VendorBlockEndDate = DateTime.MinValue;
            var service = new BlockPaymentsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.BlockAccountLegalEntitiesForPayments(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Vendor Block End Date is not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_vendor_block_end_date_is_in_the_past()
        {
            // Arrange
            var request = BuildValidBlockPaymentsRequest();
            request.VendorBlocks[0].VendorBlockEndDate = DateTime.Now.AddMinutes(-1);
            var service = new BlockPaymentsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.BlockAccountLegalEntitiesForPayments(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Vendor Block End Date must be in the future*");
        }
        
        private BlockAccountLegalEntityForPaymentsRequest BuildValidBlockPaymentsRequest()
        {
            return _fixture.Build<BlockAccountLegalEntityForPaymentsRequest>()
                .With(x => x.ServiceRequest, _fixture.Create<ServiceRequest>())
                .With(x => x.VendorBlocks, new List<VendorBlock> {
                    _fixture.Build<VendorBlock>()
                        .With(x => x.VendorBlockEndDate, DateTime.Today.AddMonths(1))
                        .Create()
                })
                .Create();
        }
    }
}
