using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Account
{
    public class WhenHandleEmploymentCheckCompletedEvent
    {
        private HandleEmploymentCheckCompletedEvent _sut;
        private Mock<IEmploymentCheckService> _mockEmploymentCheckService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockEmploymentCheckService = new Mock<IEmploymentCheckService>();
            _fixture = new Fixture();

            _mockEmploymentCheckService
                .Setup(m => m.Update(It.IsAny<UpdateRequest>()))
                .Verifiable();

            _sut = new HandleEmploymentCheckCompletedEvent(_mockEmploymentCheckService.Object);
        }

        [TestCase(true, null)]
        [TestCase(false, null)]
        [TestCase(null, "NinoNotFound")]
        [TestCase(null, "NinoFailure")]
        [TestCase(null, "NinoInvalid")]
        [TestCase(null, "PAYENotFound")]
        [TestCase(null, "PAYEFailure")]
        [TestCase(null, "NinoAndPAYENotFound")]
        [TestCase(null, "HmrcFailure")]
        public async Task Then_a_Update_Request_is_sent_to_the_EmploymentCheckService(bool? employmentResult, string errorType)
        {
            // Arrange
            var correlationId = Guid.NewGuid();
            var checkDate = DateTime.Now;

            var request = new EmploymentCheck.Types.EmploymentCheckCompletedEvent(
                correlationId,
                employmentResult,
                checkDate,
                errorType);

            // Act
            await _sut.RunEvent(request);

            // Assert
            _mockEmploymentCheckService
                .Verify(m => m.Update(It.Is<UpdateRequest>(r => 
                    r.CorrelationId == request.CorrelationId && 
                    r.DateChecked == request.CheckDate &&
                    r.Result == Map(request.EmploymentResult, request.ErrorType).ToString()))
                ,Times.Once);                
        }

        [TestCase(true, "NinoNotFound")]
        [TestCase(false, "NinoNotFound")]
        [TestCase(false, "NinoNotFound")]
        [TestCase(true, "NinoFailure")]
        [TestCase(true, "NinoInvalid")]
        [TestCase(true, "PAYENotFound")]
        [TestCase(true, "PAYEFailure")]
        [TestCase(true, "NinoAndPAYENotFound")]
        [TestCase(true, "HmrcFailure")]
        public void Then_an_exception_is_returned_for_an_invalid_request(bool? employmentResult, string errorType)
        {
            // Arrange
            var correlationId = Guid.NewGuid();
            var checkDate = DateTime.Now;

            var request = new EmploymentCheck.Types.EmploymentCheckCompletedEvent(
                correlationId,
                employmentResult,
                checkDate,
                errorType);

            // Act
            Func<Task> result = async () => await _sut.RunEvent(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage($"Unexpected Error type set when employmentResult is set : {errorType}");
            _mockEmploymentCheckService.Verify(m => m.Update(It.IsAny<UpdateRequest>()), Times.Never());
        }

        public void Then_an_exception_is_returned_for_an_invalid_error_type()
        {
            // Arrange
            var errorType = Guid.NewGuid().ToString();

            var request = new EmploymentCheck.Types.EmploymentCheckCompletedEvent(
                Guid.NewGuid(),
                null,
                DateTime.Now,
                errorType);

            // Act
            Func<Task> result = async () => await _sut.RunEvent(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage($"Unexpected Employment Check result received : {errorType}");
            _mockEmploymentCheckService.Verify(m => m.Update(It.IsAny<UpdateRequest>()), Times.Never());
        }

        [Test]
        public void Then_an_exception_is_returned_when_the_call_to_the_EmploymentCheckService_fails()
        {
            // Arrange
            _mockEmploymentCheckService
               .Setup(m => m.Update(It.IsAny<UpdateRequest>()))
               .ThrowsAsync(new Exception("Test"));

            _sut = new HandleEmploymentCheckCompletedEvent(_mockEmploymentCheckService.Object);

            var request = new EmploymentCheck.Types.EmploymentCheckCompletedEvent(
                Guid.NewGuid(),
                true,
                DateTime.Now,
                null);

            // Act
            Func<Task> result = async () => await _sut.RunEvent(request);

            // Assert
            result.Should().Throw<Exception>().WithMessage("Test");
        }

        private EmploymentCheckResult Map(bool? result, string errorType)
        {
            if (result.HasValue)
            {
                return result.Value ? EmploymentCheckResult.Employed : EmploymentCheckResult.NotEmployed;
            }

            return errorType.ToLower() switch
            {
                "ninonotfound" => EmploymentCheckResult.NinoNotFound,
                "ninofailure" => EmploymentCheckResult.NinoFailure,
                "ninoinvalid" => EmploymentCheckResult.NinoInvalid,
                "payenotfound" => EmploymentCheckResult.PAYENotFound,
                "payefailure" => EmploymentCheckResult.PAYEFailure,
                "ninoandpayenotfound" => EmploymentCheckResult.NinoAndPAYENotFound,
                "hmrcfailure" => EmploymentCheckResult.HmrcFailure,
                _ => throw new System.Exception($"Invalid 'errorType' test value {errorType}"),
            };
        }
    }
}