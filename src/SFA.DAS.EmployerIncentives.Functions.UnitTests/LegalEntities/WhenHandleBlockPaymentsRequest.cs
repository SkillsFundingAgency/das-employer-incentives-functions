﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments.Types;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.LegalEntities
{
    [TestFixture]
    public class WhenHandleBlockPaymentsRequest
    {
        private HandleBlockPaymentsRequest _sut;
        private Mock<IBlockPaymentsService> _mockBlockPaymentsService;
        private Fixture _fixture;
        private BlockAccountLegalEntityForPaymentsRequest _request;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _request = _fixture.Build<BlockAccountLegalEntityForPaymentsRequest>()
                .With(x => x.ServiceRequest, _fixture.Create<ServiceRequest>())
                .With(x => x.VendorBlocks, new List<VendorBlock> {
                    _fixture.Build<VendorBlock>()
                        .With(x => x.VendorBlockEndDate, DateTime.Today.AddMonths(1))
                        .Create(),
                    _fixture.Build<VendorBlock>()
                        .With(x => x.VendorBlockEndDate, DateTime.Today.AddMonths(2))
                        .Create()
                })
                .Create();

            _mockBlockPaymentsService = new Mock<IBlockPaymentsService>();
            
            _mockBlockPaymentsService
                .Setup(m => m.BlockAccountLegalEntitiesForPayments(_request))
                .Returns(Task.FromResult<IActionResult>(new NoContentResult()));

            _sut = new HandleBlockPaymentsRequest(_mockBlockPaymentsService.Object);
        }

        [Test]
        public async Task Then_a_RefreshLegalEntities_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(_request), Encoding.UTF8, "application/json")
            };

            // Act
            await _sut.RunHttp(request);

            // Assert
            _mockBlockPaymentsService
                .Verify(m => m.BlockAccountLegalEntitiesForPayments(It.IsAny<BlockAccountLegalEntityForPaymentsRequest>())
                    , Times.Once);
        }

        [Test]
        public async Task Then_an_OkResult_is_returned_on_success()
        {
            // Arrange
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(_request), Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _sut.RunHttp(request) as OkResult;

            // Assert
            response.Should().NotBeNull();
        }
    }
}
