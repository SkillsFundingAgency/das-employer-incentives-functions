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
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.UnitTests.Functions
{
    public class WhenHandleReinstateApplicationRequest
    {
        private HandleReinstateApplicationRequest _sut;
        private Mock<IWithdrawalService> _mockWithdrawalService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockWithdrawalService = new Mock<IWithdrawalService>();

            _mockWithdrawalService
                .Setup(m => m.Reinstate(It.IsAny<ReinstateApplicationRequest>()))
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            _sut = new HandleReinstateApplicationRequest(_mockWithdrawalService.Object);
        }

        [Test]
        public async Task Then_a_Reinstate_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var requestMessage = _fixture.Create<ReinstateApplicationRequest>();  
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestMessage), Encoding.UTF8, "application/json")
            };

            ReinstateApplicationRequest actualRequest = null;
            _mockWithdrawalService.Setup(x => x.Reinstate(It.IsAny<ReinstateApplicationRequest>()))
                .Callback<ReinstateApplicationRequest>(x => actualRequest = x);

            // Act
            await _sut.RunHttp(request);

            // Assert
            actualRequest.Should().BeEquivalentTo(requestMessage);
        }

        [Test]
        public async Task Then_an_OkResult_is_returned_on_success()
        {
            // Arrange
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(_fixture.Create<ReinstateApplicationRequest>()), Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _sut.RunHttp(request) as OkResult;

            // Assert
            response.Should().NotBeNull();
        }
    }
}