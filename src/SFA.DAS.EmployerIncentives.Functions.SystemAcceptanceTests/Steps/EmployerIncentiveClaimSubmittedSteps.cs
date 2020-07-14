using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SFA.DAS.EmployerIncentives.Functions.Commands.EmployerIncentiveClaimSubmitted;
using SFA.DAS.EmployerIncentives.Handlers;
using SFA.DAS.EmployerIncentives.Handlers.Exceptions;
using SFA.DAS.EmployerIncentives.Infrastructure.ApiClient;
using SFA.DAS.EmployerIncentives.Infrastructure.Commands;
using SFA.DAS.EmployerIncentives.Infrastructure.Configuration;
using SFA.DAS.EmployerIncentives.Infrastructure.Decorators;
using System;
using System.Linq;
using System.Net.Http;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.EmployerIncentives.Functions.SystemAcceptanceTests.Steps
{
    [Binding]
    public class EmployerIncentiveClaimSubmittedSteps
    {
        private WireMockServer _mockServer;
        private long _accountId;
        private Guid _claimId;
        private EmployerIncentiveClaimSubmittedCommandHandler _handler;
        private ExceptionContext _context;

        public EmployerIncentiveClaimSubmittedSteps(ExceptionContext context)
        {
            _context = context;
        }

        [Before]
        public void Arrange()
        {
            _accountId = 12345678;
            _claimId = Guid.NewGuid();
            var loggerFactory = new LoggerFactory();
            _mockServer = WireMockServer.StartWithAdminInterface();
            _mockServer.ResetMappings();

            var httpClient = new HttpClient { BaseAddress = new Uri(_mockServer.Urls[0])};

            var client = new CalculatePaymentApiClient(httpClient, loggerFactory.CreateLogger<CalculatePaymentApiClient>());

            _handler = new EmployerIncentiveClaimSubmittedCommandHandler(client);

        }

        [When(@"a claim is successfully submitted")]
        public void WhenAClaimIsSubmitted()
        {
            _mockServer
                .Given(Request.Create().WithPath($"/account/{_accountId}/claim/{_claimId}").UsingPost())
                .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(true));

            var command = new EmployerIncentiveClaimSubmittedCommand(_accountId, _claimId);
            _handler.Handle(command).GetAwaiter().GetResult();
        }

        [When(@"a claim is unsuccessfully submitted")]
        public void WhenAClaimIsUnsuccessfullySubmitted()
        {
            _mockServer
                .Given(Request.Create().WithPath($"/account/{_accountId}/claim/{_claimId}").UsingPost())
                .RespondWith(Response.Create()
                .WithStatusCode(500)
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(string.Empty));

            var command = new EmployerIncentiveClaimSubmittedCommand(_accountId, _claimId);
            try
            {
                _handler.Handle(command).GetAwaiter().GetResult();
            }
            catch (CommandFailureException ex)
            {
                _context.Exception = ex;
            }
        }

        [Then(@"the account and claim id should be included in the calculation request")]
        public void ThenTheAccountAndClaimIdShouldBeIncludedInTheCalculationRequest()
        {
            var scheduleRequests = _mockServer.FindLogEntries(Request.Create().WithPath($"/account/{_accountId}/claim/{_claimId}").UsingPost());
            scheduleRequests.ToList().Count().Should().Be(1);
        }

        [Then(@"an error response is returned")]
        public void ThenAnErrorIsReturned()
        {
            var scheduleRequests = _mockServer.FindLogEntries(Request.Create().WithPath($"/account/{_accountId}/claim/{_claimId}").UsingPost());
            scheduleRequests.ToList().Count().Should().Be(1);

            _context.Exception.Should().BeOfType<CommandFailureException>();
        }

        [When(@"a claim submission fails then is successful")]
        public void WhenAClaimSubmissionFailsThenIsSuccessful()
        {
            _mockServer
                .Given(Request.Create().WithPath($"/account/{_accountId}/claim/{_claimId}").UsingPost())
                .InScenario("Retry")
                .WillSetStateTo("RequestFailed")
                .RespondWith(Response.Create()
                .WithStatusCode(500)
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(string.Empty));

            _mockServer
                .Given(Request.Create().WithPath($"/account/{_accountId}/claim/{_claimId}").UsingPost())
                .InScenario("Retry")
                .WhenStateIs("RequestFailed")
                .WillSetStateTo("RequestComplete")
                .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(true));

            var command = new EmployerIncentiveClaimSubmittedCommand(_accountId, _claimId);

            var retryConfig = new Mock<IOptions<RetryPolicies>>();
            var policies = new RetryPolicies { RetryAttempts = 2, RetryWaitInMilliSeconds = 10 };
            retryConfig.Setup(x => x.Value).Returns(policies);

            var commandHandlerWithRetry = new CommandHandlerWithRetry<EmployerIncentiveClaimSubmittedCommand>(_handler, new Policies(retryConfig.Object));
            commandHandlerWithRetry.Handle(command).GetAwaiter().GetResult();
        }

        [Then(@"the claim should submit on the second attempt")]
        public void ThenTheClaimShouldSubmitOnTheSecondAttempt()
        {
            var scheduleRequests = _mockServer.FindLogEntries(Request.Create().WithPath($"/account/{_accountId}/claim/{_claimId}").UsingPost());
            scheduleRequests.ToList().Count().Should().Be(2);
        }

    }
}
