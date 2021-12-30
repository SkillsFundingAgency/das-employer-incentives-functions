using AutoFixture;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs.Types;
using SFA.DAS.EmployerIncentives.Types;
using System;
using System.Net;
using System.Threading.Tasks;

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
                Type = JobType.RefreshEmploymentCheck,
                Data = new System.Collections.Generic.Dictionary<string, string>
                    {
                        { "AccountLegalEntityId", refreshRequest.AccountLegalEntityId.ToString() },
                        { "ULN", refreshRequest.ULN.ToString() },
                        { "ServiceRequest", JsonConvert.SerializeObject(refreshRequest.ServiceRequest) }
                    }
            };

            // Act
            await _sut.Refresh(refreshRequest);

            // Assert
            _testClient.VerifyPutAsAsync($"jobs", expected, Times.Once());
        }
    }
}