using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.ValidationOverrides;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.ValidationOverrides.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.UnitTests.Functions
{
    public class WhenHandleValidationOverrideRequest
    {
        private HandleValidationOverrideRequest _sut;
        private Mock<IValidationOverrideService> _mockValidationOverrideService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockValidationOverrideService = new Mock<IValidationOverrideService>();

            _mockValidationOverrideService
                .Setup(m => m.Add(It.IsAny<IEnumerable<ValidationOverride>>()))
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            _sut = new HandleValidationOverrideRequest(_mockValidationOverrideService.Object);
        }

        [Test]
        public async Task Then_a_Validation_Override_Request_is_sent_to_the_EmployerIncentivesService()
        {
            // Arrange
            var requestMessage = _fixture.Create<IEnumerable<ValidationOverride>>();  
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestMessage), Encoding.UTF8, "application/json")
            };

            // Act
            await _sut.RunHttp(request);

            // Assert
            _mockValidationOverrideService
                .Verify(m => m.Add(It.Is<IEnumerable<ValidationOverride>>(v => v.Count() == 3))
                , Times.Once);
        }

        [Test]
        public async Task Then_an_OkResult_is_returned_on_success()
        {
            // Arrange
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(_fixture.Create<IEnumerable<ValidationOverride>>()), Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _sut.RunHttp(request) as OkResult;

            // Assert
            response.Should().NotBeNull();
        }
    }
}