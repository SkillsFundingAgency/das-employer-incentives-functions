using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.VendorRegistrationForm
{
    public class WhenRefresh
    {
        private IVendorRegistrationFormService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _baseAddress = new Uri("http://localhost");
            _testClient = new TestHttpClient(_baseAddress);
            _testClient.SetUpPutAsAsync(HttpStatusCode.OK);
            _sut = new VendorRegistrationFormService(_testClient);
        }

        [Test]
        public async Task Then_the_Apim_endpoint_is_invoked()
        {
            // Arrange

            // Act
            await _sut.Refresh();

            // Assert
            _testClient.VerifyPatchAsAsync("legalentities/vendorregistrationform", Times.Once());
        }
    }
}