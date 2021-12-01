using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.EmploymentCheck
{
    public class WhenUpdate
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
            var correlationId = _fixture.Create<Guid>();
            var updateRequest = _fixture
                .Build<UpdateRequest>()
                .With(r => r.CorrelationId, correlationId)
                .With(r => r.Result, EmploymentCheckResult.Employed.ToString())
                .Create();

            // Act
            await _sut.Update(updateRequest);

            // Assert
            _testClient.VerifyPutAsAsync($"employmentchecks/{correlationId}", updateRequest, Times.Once());
        }
    }
}