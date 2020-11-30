using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.CollectionCalendar
{
    [TestFixture]
    public class WhenActivatingCollectionCalendarPeriod
    {
        private CollectionCalendarService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _baseAddress = new Uri("http://localhost");
            _testClient = new TestHttpClient(_baseAddress);
            _testClient.SetUpPatchAsAsync(HttpStatusCode.OK);

            _sut = new CollectionCalendarService(_testClient);

            _fixture = new Fixture();
        }

        [Test]
        public async Task Then_API_is_invoked()
        {
            await _sut.ActivatePeriod(_fixture.Create<short>(), _fixture.Create<byte>());

            _testClient.VerifyPatchAsAsync("collectionCalendar/period/activate", Times.Once());
        }
    }
}
