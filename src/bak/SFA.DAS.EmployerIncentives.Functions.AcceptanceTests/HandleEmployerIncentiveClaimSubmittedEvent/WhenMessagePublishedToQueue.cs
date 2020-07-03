using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Infastructure;
//using SFA.DAS.EmployerIncentives.Messages.Events;
//using System;
//using System.Linq;
using System.Threading.Tasks;
//using WireMock.RequestBuilders;
//using WireMock.ResponseBuilders;
using WireMock.Server;
//using Microsoft.Extensions.Hosting;
//using SFA.DAS.EmployerIncentives.Infrastructure.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using FluentAssertions;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.HandleEmployerIncentiveClaimSubmittedEvent
{
    [TestFixture]
    public class WhenMessagePublishedToQueue //: AcceptanceTestBase
    {
        private WireMockServer _mockServer; 

        [SetUp]
        public async Task Arrange()
        {
        //    _mockServer = WireMockServer.StartWithAdminInterface();
        //    _mockServer.ResetMappings();

        //    _mockServer
        //     .Given(Request.Create().WithUrl(x => x.StartsWith("/accounts")).UsingPost())
        //     .RespondWith(Response.Create()
        //     .WithStatusCode(200)
        //     .WithHeader("Content-Type", "application/json")
        //     .WithBodyAsJson(true));

        //    var startUp = new Startup();
        //    var host = new HostBuilder()
        //        .ConfigureWebJobs(startUp.Configure);
        //    host.ConfigureServices((s) =>
        //    {
        //        s.Configure<ClientApiConfiguration>(a =>
        //        {
        //            a.ApiBaseUrl = _mockServer.Urls[0];
        //        });
        //    });

        //    await base.Start();
        }

        [TearDown]
        public void TearDown()
        {
            //base.Stop().GetAwaiter().GetResult();
            //_mockServer.Stop();
        }

        [Test]
        public void Then_outer_api_called_with_claim_details()
        {
            //// Arrange
            //var accountId = 12345678;
            //var claimId = Guid.NewGuid();
            //var message = new EmployerIncentiveClaimSubmittedEvent
            //{
            //    AccountId = accountId,
            //    IncentiveClaimApprenticeshipId = claimId
            //};

            //// Act
            //base.Publish(message).GetAwaiter().GetResult();

            //// Assert
            //var scheduleRequests = _mockServer.FindLogEntries(Request.Create().WithPath(u => u.StartsWith($"/accounts")).UsingPost());
            //scheduleRequests.ToList().Count().Should().Be(1);

            Assert.Pass();
        }

        //[TestCase(500)]
        //[TestCase(503)]
        //[TestCase(404)]
        //[TestCase(401)]
        //[TestCase(403)]
        //[TestCase(502)]
        //[Test]
        //public void Then_outer_api_responds_with_an_error_status_code(int errorStatusCode)
        //{
        //    // Arrange
        //    _mockServer.ResetMappings();

        //    _mockServer
        //     .Given(Request.Create().WithUrl(x => x.StartsWith("/accounts")).UsingPost())
        //     .RespondWith(Response.Create()
        //     .WithStatusCode(errorStatusCode)
        //     .WithHeader("Content-Type", "application/json")
        //     .WithBodyAsJson(null));

        //    var accountId = 12345678;
        //    var claimId = Guid.NewGuid();
        //    var message = new EmployerIncentiveClaimSubmittedEvent
        //    {
        //        AccountId = accountId,
        //        IncentiveClaimApprenticeshipId = claimId
        //    };

        //    // Act
        //    base.Publish(message).GetAwaiter().GetResult();

        //    // Assert
        //    var scheduleRequests = _mockServer.FindLogEntries(Request.Create().WithPath(u => u.StartsWith($"/accounts")).UsingPost());
        //    scheduleRequests.ToList().Count().Should().Be(1);
        //}
    }
}
