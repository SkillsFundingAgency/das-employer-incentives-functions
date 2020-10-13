using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
using SFA.DAS.EmployerIncentives.Infrastructure.Extensions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.VendorRegistrationForm
{
    public class WhenUpdate
    {
        private IVendorRegistrationFormService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;

        [SetUp]
        public void Setup()
        {
            _baseAddress = new Uri("http://localhost");
            _testClient = new TestHttpClient(_baseAddress);
            _testClient.SetUpPatchAsAsync(HttpStatusCode.Created);

            _sut = new VendorRegistrationFormService(_testClient);
        }

        [Test]
        public async Task Then_API_is_invoked_with_LastRunDateTime_as_FromDateTime()
        {
            // Arrange
            var lastRunDateTime = DateTime.UtcNow.AddHours(-1);

            // Act
            await _sut.Update(lastRunDateTime);

            // Assert
            _testClient.VerifyPatchAsAsync($"legalentities/vendorregistrationform/status?from={lastRunDateTime.ToIsoDateTime()}", Times.Once());
        }

    }
}
