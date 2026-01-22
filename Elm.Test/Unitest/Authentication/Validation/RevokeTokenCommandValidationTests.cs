using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Validations.Authentication;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Authentication.Validation
{
    public class RevokeTokenCommandValidationTests
    {
        private readonly RevokeTokenCommandValidation _validator;

        public RevokeTokenCommandValidationTests()
        {
            _validator = new RevokeTokenCommandValidation();
        }

        [Fact]
        public void Validate_WhenTokenIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RevokeTokenCommand(" ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Token)
                .WithErrorMessage("الرمز المميز مطلوب");
        }

        [Fact]
        public void Validate_WhenTokenIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RevokeTokenCommand(null!);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Token)
                .WithErrorMessage("الرمز المميز مطلوب");
        }

        [Fact]
        public void Validate_WhenTokenIsWhitespace_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RevokeTokenCommand("   ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Token)
                .WithErrorMessage("الرمز المميز مطلوب");
        }

        [Fact]
        public void Validate_WhenTokenIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new RevokeTokenCommand("valid-token-to-revoke");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Token);
        }

        [Theory]
        [InlineData("token123")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9")]
        [InlineData("revoke-this-token")]
        public void Validate_WithValidTokens_ShouldNotHaveAnyValidationErrors(string token)
        {
            // Arrange
            var command = new RevokeTokenCommand(token);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
