using System;
using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.EmployerVendorId
{
    public class WhenAddingEmployerVendorId
    {
        private EmployerVendorIdService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;

        [SetUp]
        public void Setup()
        {
            _baseAddress = new Uri("http://localhost");
            _testClient = new TestHttpClient(_baseAddress);
            _testClient.SetUpPatchAsAsync(HttpStatusCode.Created);

            _sut = new EmployerVendorIdService(_testClient);
        }

        [Test]
        public async Task Then_API_is_invoked_with_hashedLegalEntityId()
        {
            var hashedLegalEntityId = "ABC123";

            await _sut.GetAndAddEmployerVendorId(hashedLegalEntityId);

            _testClient.VerifyPutAsAsync($"legalentities/{hashedLegalEntityId}/employervendorid", Times.Once());
        }
    }
}