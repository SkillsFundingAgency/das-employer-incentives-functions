using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.RecalculateEarnings;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.RecalculateEarnings.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.UnitTests.Services.RecalculateEarnings
{
    [TestFixture]
    public class WhenRecalculatingEarnings
    {
        private RecalculateEarningsService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;
        private Fixture _fixture;
        
        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _baseAddress = new Uri(@"http://localhost");
            _testClient = new TestHttpClient(_baseAddress);

            _testClient.SetUpPostAsAsync(HttpStatusCode.OK);

            _sut = new RecalculateEarningsService(_testClient);
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client()
        {
            // Arrange
            var request = BuildValidRecalculateEarningsRequest();

            // Act
            await _sut.RecalculateEarnings(request);

            // Assert
            _testClient.VerifyPostAsAsync($"earningsRecalculations", request, Times.Once());
        }

        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.BadRequest)]
        public void Then_a_PausePaymentServiceException_is_thrown_when_the_client_returns(HttpStatusCode clientResponse)
        {
            // Arrange
            var request = BuildValidRecalculateEarningsRequest();
            _testClient.SetUpPostAsAsync(clientResponse);

            // Act
            Func<Task> result = async () => await _sut.RecalculateEarnings(request);

            // Assert
            result.Should().Throw<RecalculateEarningsServiceException>();
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_incentive_learner_identifiers_are_null()
        {
            // Arrange
            var request = new RecalculateEarningsRequest {IncentiveLearnerIdentifiers = null};
            var service = new RecalculateEarningsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.RecalculateEarnings(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Incentive Learner Identifiers are not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_incentive_learner_identifiers_are_not_set()
        {
            // Arrange
            var request = new RecalculateEarningsRequest { IncentiveLearnerIdentifiers = new List<IncentiveLearnerIdentifierDto>() };
            var service = new RecalculateEarningsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.RecalculateEarnings(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Incentive Learner Identifiers are not set*");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_incentive_learner_identifier_account_legal_entity_id_is_not_set()
        {
            // Arrange
            var request = BuildValidRecalculateEarningsRequest();
            request.IncentiveLearnerIdentifiers[0].AccountLegalEntityId = 0;
            var service = new RecalculateEarningsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.RecalculateEarnings(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Incentive Learner Identifier Account Legal Entity not set*");
        }


        [Test]
        public void Then_an_exception_is_thrown_when_the_incentive_learner_identifier_uln_is_not_set()
        {
            // Arrange
            var request = BuildValidRecalculateEarningsRequest();
            request.IncentiveLearnerIdentifiers[0].ULN = 0;
            var service = new RecalculateEarningsServiceValidation(_sut);

            // Act
            Func<Task> result = async () => await service.RecalculateEarnings(request);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Incentive Learner Identifier ULN not set*");
        }

        private RecalculateEarningsRequest BuildValidRecalculateEarningsRequest()
        {
            return _fixture.Build<RecalculateEarningsRequest>()
                .With(x => x.IncentiveLearnerIdentifiers, _fixture.CreateMany<IncentiveLearnerIdentifierDto>(3).ToList())
                .Create();
        }
    }

}
