using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.Commands.EmployerIncentiveClaimSubmitted;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.Commands.Tests
{
    [TestFixture]
    public class WhenEmployerIncentiveClaimSubmittedCommandValidated
    {
        private EmployerIncentiveClaimSubmittedCommandValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new EmployerIncentiveClaimSubmittedCommandValidator();
        }

        [Test]
        public async Task Then_command_is_valid_with_a_claim_id()
        {   
            // Arrange
            var command = new EmployerIncentiveClaimSubmittedCommand(Guid.NewGuid());

            // Act
            var validationResult = await _validator.Validate(command);

            // Assert
            validationResult.IsValid().Should().BeTrue();
        }

        [Test]
        public async Task Then_command_is_invalid_with_empty_guid_for_claim_id()
        {
            // Arrange
            var command = new EmployerIncentiveClaimSubmittedCommand(Guid.Empty);

            // Act
            var validationResult = await _validator.Validate(command);

            // Assert
            validationResult.IsValid().Should().BeFalse();
        }
    }
}
