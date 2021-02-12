using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.LegalEntities
{
    public class WhenRefreshLegalEntities
    {
        private LegalEntitiesService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _baseAddress = new Uri(@"http://localhost");
            _testClient = new TestHttpClient(_baseAddress);

            _testClient.SetUpPostAsAsync(HttpStatusCode.Created);

            _sut = new LegalEntitiesService(_testClient);
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client()
        {
            // Arrange
            var accountId = _fixture.Create<int>();
            var addRequest = _fixture
                .Build<AddRequest>()
                .With(r => r.AccountId, accountId)
                .Create();

            // Act
            await _sut.Add(addRequest);

            // Assert
            _testClient.VerifyPostAsAsync($"accounts/{accountId}/legalEntities", addRequest, Times.Once());
        }
    }
}