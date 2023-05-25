using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.LegalEntities
{
    public class WhenRefresh
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

            _testClient.SetUpPutAsAsync(HttpStatusCode.OK);

            _sut = new LegalEntitiesService(_testClient);
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client()
        {
            // Arrange
            var pageNumber = _fixture.Create<int>();
            var pageSize = _fixture.Create<int>();

            // Act
            await _sut.Refresh(pageNumber, pageSize);

            // Assert
            _testClient.VerifyPutAsAsync($"legalentities/refresh?pageNumber={pageNumber}&pageSize={pageSize}", Times.Once());
        }

    }
}