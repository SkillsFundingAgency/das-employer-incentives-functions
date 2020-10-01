//using Microsoft.Extensions.Logging;
//using Moq;
//using NUnit.Framework;
//using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
//using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;
//using SFA.DAS.EmployerIncentives.Infrastructure.Extensions;
//using System;
//using System.Net;
//using System.Threading.Tasks;

//namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.VendorRegistrationFormService
//{

//    public class WhenRefreshVendorRegistrationFormStatuses
//    {
//        private IVendorRegistrationFormService _sut;
//        private Uri _baseAddress;
//        private TestHttpClient _testClient;

//        [SetUp]
//        public void Setup()
//        {
//            _baseAddress = new Uri("http://localhost");
//            _testClient = new TestHttpClient(_baseAddress);
//            _testClient.SetUpPatchAsAsync(HttpStatusCode.Created);

//            _sut = new Functions.LegalEntities.Services.LegalEntities.VendorRegistrationFormService(_testClient);
//        }

//        [Test]
//        public async Task Then_API_is_invoked_with_LastRunDateTime_as_FromDateTime_and_current_DateTime_as_ToDateTime_and_last_Run_DateTime_is_updated()
//        {
//            // Arrange
//            var lastRunDateTime = DateTime.UtcNow.AddHours(-1);
//            _mockVrfRefreshRepo.Setup(x => x.GetLastRunDateTime()).ReturnsAsync(lastRunDateTime);
//            var currentDateTime = DateTime.UtcNow;
//            _mockDateTimeProvider.Setup(x => x.GetCurrentDateTime()).ReturnsAsync(currentDateTime);

//            // Act
//            await _sut.RefreshStatuses();

//            // Assert
//            _testClient.VerifyPatchAsAsync($"legalentities/vendorregistrationform/status?from={lastRunDateTime.ToIsoDateTime()}&to={currentDateTime.ToIsoDateTime()}", Times.Once());
//            _mockVrfRefreshRepo.Verify(x => x.UpdateLastRunDateTime(currentDateTime), Times.Once);
//        }

//    }
//}
