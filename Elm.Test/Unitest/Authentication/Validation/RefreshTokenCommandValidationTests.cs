using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Validations.Authentication;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Authentication.Validation
{
    public class RefreshTokenCommandValidationTests
    {
        private readonly RefreshTokenCommandValidation _validator;

        public RefreshTokenCommandValidationTests()
        {
            _validator = new RefreshTokenCommandValidation();
        }

        [Fact]
        public void Validate_WhenTokenIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RefreshTokenCommand(" ");

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
            var command = new RefreshTokenCommand(null!);

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
            var command = new RefreshTokenCommand("   ");

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
            var command = new RefreshTokenCommand("valid-refresh-token-123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Token);
        }

        [Theory]
        [InlineData("token123")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9")]
        [InlineData("refresh-token-uuid-format")]
        public void Validate_WithValidTokens_ShouldNotHaveAnyValidationErrors(string token)
        {
            // Arrange
            var command = new RefreshTokenCommand(token);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
