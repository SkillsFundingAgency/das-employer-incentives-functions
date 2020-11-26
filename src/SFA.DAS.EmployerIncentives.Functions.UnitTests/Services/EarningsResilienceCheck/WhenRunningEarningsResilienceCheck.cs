using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.EarningsResilienceCheck
{
    public class WhenRunningEarningsResilienceCheck
    {
        private EarningsResilienceCheckService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;

        [SetUp]
        public void Setup()
        {
            _baseAddress = new Uri("http://localhost");
            _testClient = new TestHttpClient(_baseAddress);
            _testClient.SetUpPostAsAsync(HttpStatusCode.OK);

            _sut = new EarningsResilienceCheckService(_testClient);
        }

        [Test]
        public async Task Then_API_is_invoked()
        {
            await _sut.Update();

            _testClient.VerifyPostAsAsync("earnings-resilience-check", Times.Once());
        }
    }
}
