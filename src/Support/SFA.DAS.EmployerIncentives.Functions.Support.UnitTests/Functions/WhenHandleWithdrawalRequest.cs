using System.Linq;
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
    public class WhenHandleWithdrawalRequest
    {
        private HandleWithdrawalRequest _sut;
        private Mock<IWithdrawalService> _mockWithdrawalService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockWithdrawalService = new Mock<IWithdrawalService>();

            _mockWithdrawalService
                .Setup(m => m.Withdraw(It.IsAny<WithdrawRequest>()))
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            _sut = new HandleWithdrawalRequest(_mockWithdrawalService.Object);
        }

        [Test]
        public async Task Then_a_Withdrawal_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var applications = _fixture.CreateMany<Application>(5).ToArray();
            var requestMessage = _fixture.Build<WithdrawRequest>()
                .With(x => x.Applications, applications).Create();
            
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestMessage), Encoding.UTF8, "application/json")
            };

            // Act
            await _sut.RunHttp(request);

            // Assert
            _mockWithdrawalService
                .Verify(m => m.Withdraw(It.Is<WithdrawRequest>(r =>
                    r.ServiceRequest.TaskId == requestMessage.ServiceRequest.TaskId &&
                    r.ServiceRequest.DecisionReference == requestMessage.ServiceRequest.DecisionReference &&
                    r.ServiceRequest.TaskCreatedDate == requestMessage.ServiceRequest.TaskCreatedDate
                ))
                , Times.Once);

            foreach(var application in applications)
            {
                _mockWithdrawalService
                    .Verify(m => m.Withdraw(It.Is<WithdrawRequest>(r =>
                        r.Applications.FirstOrDefault(x => 
                            x.AccountLegalEntityId == application.AccountLegalEntityId && x.ULN == application.ULN) 
                        != null
                    ))
                    , Times.Once);
            }
        }

        [Test]
        public async Task Then_an_OkResult_is_returned_on_success()
        {
            // Arrange
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(_fixture.Create<WithdrawRequest>()), Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _sut.RunHttp(request) as OkResult;

            // Assert
            response.Should().NotBeNull();
        }
    }
}