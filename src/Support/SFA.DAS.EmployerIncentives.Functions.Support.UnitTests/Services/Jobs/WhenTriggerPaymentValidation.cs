using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.UnitTests.Services.Jobs
{
    public class WhenTriggerPaymentValidation
    {
        private JobsService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;

        [SetUp]
        public void Setup()
        {
            _baseAddress = new Uri(@"http://localhost");
            _testClient = new TestHttpClient(_baseAddress);

            _testClient.SetUpPutAsAsync(HttpStatusCode.OK);

            _sut = new JobsService(_testClient);
        }

        [Test]
        public async Task Then_the_payment_validation_job_request_is_forwarded_to_the_client()
        {
            // Arrange
            var expected = new JobRequest
            {
                Type = JobType.PaymentValidation,
                Data = new Dictionary<string, string>()
            };

            // Act
            await _sut.TriggerPaymentValidation();

            // Assert
            _testClient.VerifyPutAsAsync("jobs", expected, Times.Once());
        }
    }
}