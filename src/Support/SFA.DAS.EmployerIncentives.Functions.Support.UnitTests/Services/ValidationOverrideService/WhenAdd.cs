using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.ValidationOverrides;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.ValidationOverrides.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.UnitTests.Services.ValidationOverrideService
{
    public class WhenAdd
    {
        private IValidationOverrideService _sut;
        private Uri _baseAddress;
        private TestHttpClient _testClient;
        private Fixture _fixture;
        private List<ValidationOverride> _validationOverrideRequests;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _baseAddress = new Uri(@"http://localhost");
            _testClient = new TestHttpClient(_baseAddress);

            _testClient.SetUpPostAsAsync(HttpStatusCode.OK);

            _sut = new ValidationOverrideServiceValidation(new Support.Services.ValidationOverrides.ValidationOverrideService(_testClient));

            _validationOverrideRequests = new List<ValidationOverride>
            {
                _fixture
                .Build<ValidationOverride>()
                .With(v => v.ValidationSteps, new List<ValidationStep>() {
                    new ValidationStep()
                    {
                        ValidationType = ValidationType.IsInLearning,
                        ExpiryDate = DateTime.UtcNow.AddDays(10)
                    }}.ToArray())
                .Create()
            };
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client()
        {
            // Arrange           

            // Act
            await _sut.Add(_validationOverrideRequests);

            // Assert
            _testClient.VerifyPostAsAsync($"validation-overrides", new ValidationOverrideRequest() { ValidationOverrides = _validationOverrideRequests.ToArray() }, Times.Once());
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_AccountLegalEntityId_is_duplicated()
        {
            // Arrange
            _validationOverrideRequests.Add(_validationOverrideRequests[0]);
            _validationOverrideRequests[1].ULN = _fixture.Create<long>();

            // Act
            Func<Task> result = async () => await _sut.Add(_validationOverrideRequests);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Duplicate ValidationOverride entries exist. The combination of AccountLegalEntityId and ULN should be unique.");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_Uln_is_duplicated()
        {
            // Arrange
            _validationOverrideRequests.Add(_validationOverrideRequests[0]);
            _validationOverrideRequests[1].AccountLegalEntityId = _fixture.Create<long>();

            // Act
            Func<Task> result = async () => await _sut.Add(_validationOverrideRequests);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage("Duplicate ValidationOverride entries exist. The combination of AccountLegalEntityId and ULN should be unique.");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_validationSteps_are_not_set()
        {
            // Arrange
            _validationOverrideRequests[0].ValidationSteps = null;

            // Act
            Func<Task> result = async () => await _sut.Add(_validationOverrideRequests);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage($"ValidationSteps not set or invalid for AccountLegalEntityId : {_validationOverrideRequests[0].AccountLegalEntityId}, ULN : {_validationOverrideRequests[0].ULN}");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_validationSteps_are_empty()
        {
            // Arrange
            _validationOverrideRequests[0].ValidationSteps = new List<ValidationStep>().ToArray();

            // Act
            Func<Task> result = async () => await _sut.Add(_validationOverrideRequests);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage($"ValidationSteps not set or invalid for AccountLegalEntityId : {_validationOverrideRequests[0].AccountLegalEntityId}, ULN : {_validationOverrideRequests[0].ULN}");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ULN_is_not_set()
        {
            // Arrange
            _validationOverrideRequests[0].ULN = default;

            // Act
            Func<Task> result = async () => await _sut.Add(_validationOverrideRequests);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage($"ULN not set for AccountLegalEntityId : {_validationOverrideRequests[0].AccountLegalEntityId}, ULN : {_validationOverrideRequests[0].ULN}");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_AccountLegalEntityId_is_not_set()
        {
            // Arrange
            _validationOverrideRequests[0].AccountLegalEntityId = default;

            // Act
            Func<Task> result = async () => await _sut.Add(_validationOverrideRequests);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage($"AccountLegalEntityId not set for AccountLegalEntityId : {_validationOverrideRequests[0].AccountLegalEntityId}, ULN : {_validationOverrideRequests[0].ULN}");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ValidationStep_is_duplicated()
        {
            // Arrange
            _validationOverrideRequests[0].ValidationSteps = new List<ValidationStep> { _validationOverrideRequests[0].ValidationSteps[0], _validationOverrideRequests[0].ValidationSteps[0] }.ToArray();

            // Act
            Func<Task> result = async () => await _sut.Add(_validationOverrideRequests);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage($"Duplicate ValidationSteps exist for AccountLegalEntityId : {_validationOverrideRequests[0].AccountLegalEntityId}, ULN : {_validationOverrideRequests[0].ULN}");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ValidationStep_type_is_not_set()
        {
            // Arrange
            _validationOverrideRequests[0].ValidationSteps[0].ValidationType = ValidationType.NotSet;

            // Act
            Func<Task> result = async () => await _sut.Add(_validationOverrideRequests);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage($"ValidationStep invalid for AccountLegalEntityId : {_validationOverrideRequests[0].AccountLegalEntityId}, ULN : {_validationOverrideRequests[0].ULN}, Error : ValidationType not set or invalid");
        }

        [Test]
        public void Then_an_exception_is_thrown_when_the_ExpiryDate_is_before_today()
        {
            // Arrange
            _validationOverrideRequests[0].ValidationSteps[0].ExpiryDate = DateTime.UtcNow.AddDays(-1);

            // Act
            Func<Task> result = async () => await _sut.Add(_validationOverrideRequests);

            // Assert
            result.Should().Throw<ArgumentException>().WithMessage($"ValidationStep invalid for AccountLegalEntityId : {_validationOverrideRequests[0].AccountLegalEntityId}, ULN : {_validationOverrideRequests[0].ULN}, Error : ExpiryDate '{_validationOverrideRequests[0].ValidationSteps[0].ExpiryDate:yyyy'-'MM'-'dd'T'HH':'mm'Z'}' can not be in the past");
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client_with_the_task_date_set_when_service_request_not_set()
        {
            // Arrange
            _validationOverrideRequests[0].ServiceRequest = null;

            // Act
            await _sut.Add(_validationOverrideRequests);

            // Assert
            _testClient.VerifyPostAsAsync($"validation-overrides", new ValidationOverrideRequest() { ValidationOverrides = _validationOverrideRequests.ToArray() }, Times.Once());
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client_when_the_remove_is_set_to_true()
        {
            // Arrange
            _validationOverrideRequests[0].ValidationSteps[0].Remove = true;

            // Act
            await _sut.Add(_validationOverrideRequests);

            // Assert
            _testClient.VerifyPostAsAsync($"validation-overrides", new ValidationOverrideRequest() { ValidationOverrides = _validationOverrideRequests.ToArray() }, Times.Once());
        }

        [Test]
        public async Task Then_the_request_is_forwarded_to_the_client_when_the_remove_is_not_set()
        {
            // Arrange
            _validationOverrideRequests[0].ValidationSteps[0].Remove = null;

            // Act
            await _sut.Add(_validationOverrideRequests);

            // Assert
            _testClient.VerifyPostAsAsync($"validation-overrides", new ValidationOverrideRequest() { ValidationOverrides = _validationOverrideRequests.ToArray() }, Times.Once());
        }

        [Test]
        public void Then_an_exception_is_not_thrown_when_the_ExpiryDate_is_before_today_if_the_remove_flag_is_set()
        {
            // Arrange
            _validationOverrideRequests[0].ValidationSteps[0].ExpiryDate = DateTime.UtcNow.AddDays(-1);
            _validationOverrideRequests[0].ValidationSteps[0].Remove = true;

            // Act
            Func <Task> result = async () => await _sut.Add(_validationOverrideRequests);

            // Assert
            result.Should().NotThrow<ArgumentException>();
        }

    }
}