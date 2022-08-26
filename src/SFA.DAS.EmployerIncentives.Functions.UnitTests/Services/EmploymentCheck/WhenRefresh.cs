using AutoFixture;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs.Types;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Types;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.EmploymentCheck
{
    public class WhenRefresh
    {
        private EmploymentCheckService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _baseAddress = new Uri(@"http://localhost");
            _testClient = new TestHttpClient(_baseAddress);

            _testClient.SetUpPutAsAsync(HttpStatusCode.OK);

            _sut = new EmploymentCheckService(_testClient);
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client()
        {
            // Arrange            
            var refreshRequest = _fixture
                .Build<EmploymentCheckRequest>()
                .Create();

            var expected = new JobRequest
            {
                Type = JobType.RefreshEmploymentChecks,
                Data = new System.Collections.Generic.Dictionary<string, string>
                    {
                        { "CheckType", refreshRequest.CheckType.ToString() },
                        { "Applications", JsonConvert.SerializeObject(refreshRequest.Applications) },
                        { "ServiceRequest", JsonConvert.SerializeObject(refreshRequest.ServiceRequest) }
                    }
            };

            // Act
            await _sut.Refresh(refreshRequest);

            // Assert
            _testClient.VerifyPutAsAsync($"jobs", expected, Times.Once());
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Then_the_request_is_invalid_if_the_check_type_is_not_set(string value)
        {
            // Arrange
            var refreshRequest = _fixture.Build<EmploymentCheckRequest>()
                .With(x => x.CheckType, value)
                .Create();
            
            var service = new EmploymentCheckValidation(_sut);
            
            // Act
            Func<Task> result = async () => await service.Refresh(refreshRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Employment check type not set*");
        }

        [Test]
        public void Then_the_request_is_invalid_if_the_check_type_is_invalid()
        {
            // Arrange
            var refreshRequest = _fixture.Build<EmploymentCheckRequest>()
                .With(x => x.CheckType, "Test")
                .Create();
            
            var service = new EmploymentCheckValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Refresh(refreshRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Invalid employment check type*");
        }

        [Test]
        public void Then_the_request_is_invalid_if_the_applications_are_not_set()
        {
            // Arrange
            var refreshRequest = _fixture.Build<EmploymentCheckRequest>()
                .With(x => x.CheckType, RefreshEmploymentCheckType.EmployedAt365DaysCheck.ToString())
                .Without(x => x.Applications)
                .Create();
            
            var service = new EmploymentCheckValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Refresh(refreshRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Applications not set*");
        }

        [Test]
        public void Then_the_request_is_invalid_if_the_applications_are_empty()
        {
            // Arrange
            var refreshRequest = _fixture.Build<EmploymentCheckRequest>()
                .With(x => x.CheckType, RefreshEmploymentCheckType.EmployedAt365DaysCheck.ToString())
                .With(x => x.Applications, Array.Empty<Application>())
                .Create();
            
            var service = new EmploymentCheckValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Refresh(refreshRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Applications not set*");
        }

        [Test]
        public void Then_the_request_is_invalid_if_the_service_request_is_not_set()
        {
            // Arrange
            var refreshRequest = _fixture.Build<EmploymentCheckRequest>()
                .With(x => x.CheckType, RefreshEmploymentCheckType.EmployedAt365DaysCheck.ToString())
                .Without(x => x.ServiceRequest)
                .Create();
            
            var service = new EmploymentCheckValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Refresh(refreshRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ServiceRequest not set*");
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Then_the_request_is_invalid_if_the_service_request_task_id_is_not_set(string value)
        {
            // Arrange
            var refreshRequest = _fixture.Build<EmploymentCheckRequest>()
                .With(x => x.CheckType, RefreshEmploymentCheckType.InitialEmploymentChecks.ToString())
                .Create();
            refreshRequest.ServiceRequest.TaskId = value;

            var service = new EmploymentCheckValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Refresh(refreshRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ServiceRequest TaskId not set*");
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Then_the_request_is_invalid_if_the_service_request_decision_reference_is_not_set(string value)
        {
            // Arrange
            var refreshRequest = _fixture.Build<EmploymentCheckRequest>()
                .With(x => x.CheckType, RefreshEmploymentCheckType.InitialEmploymentChecks.ToString())
                .Create();
            refreshRequest.ServiceRequest.DecisionReference = value;

            var service = new EmploymentCheckValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Refresh(refreshRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ServiceRequest DecisionReference not set*");
        }

        [Test]
        public void Then_the_request_is_invalid_if_the_service_request_created_date_is_not_set()
        {
            // Arrange
            var refreshRequest = _fixture.Build<EmploymentCheckRequest>()
                .With(x => x.CheckType, RefreshEmploymentCheckType.InitialEmploymentChecks.ToString())
                .Create();
            refreshRequest.ServiceRequest.TaskCreatedDate = null;

            var service = new EmploymentCheckValidation(_sut);

            // Act
            Func<Task> result = async () => await service.Refresh(refreshRequest);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("ServiceRequest TaskCreatedDate not set*");
        }
    }
}