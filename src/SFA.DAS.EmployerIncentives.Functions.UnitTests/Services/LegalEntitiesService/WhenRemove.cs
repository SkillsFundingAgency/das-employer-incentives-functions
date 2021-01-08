using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.LegalEntities
{
    public class WhenRemove
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

            _testClient.SetUpDeleteAsAsync(HttpStatusCode.OK);

            _sut = new LegalEntitiesService(_testClient);
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client()
        {
            // Arrange
            var accountId = _fixture.Create<int>();
            var accountLegalEntityId = _fixture.Create<int>();
            var removeRequest = _fixture
                .Build<RemoveRequest>()
                .With(r => r.AccountId, accountId)
                .With(r => r.AccountLegalEntityId, accountLegalEntityId)
                .Create();

            // Act
            await _sut.Remove(removeRequest);

            // Assert
            _testClient.VerifyDeleteAsync($"accounts/{accountId}/legalEntities/{accountLegalEntityId}", Times.Once());
        }
    }
}