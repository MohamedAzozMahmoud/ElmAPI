using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Validations.Authentication;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Authentication.Validation
{
    public class DeleteCommandValidationTests
    {
        private readonly DeleteCommandValidation _validator;

        public DeleteCommandValidationTests()
        {
            _validator = new DeleteCommandValidation();
        }

        [Fact]
        public void Validate_WhenUserIdIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new DeleteCommand(" ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("معرف المستخدم مطلوب");
        }

        [Fact]
        public void Validate_WhenUserIdIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new DeleteCommand(null!);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("معرف المستخدم مطلوب");
        }

        [Fact]
        public void Validate_WhenUserIdIsWhitespace_ShouldHaveValidationError()
        {
            // Arrange
            var command = new DeleteCommand("   ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("معرف المستخدم مطلوب");
        }

        [Fact]
        public void Validate_WhenUserIdIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new DeleteCommand("user-123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("user-id-123")]
        [InlineData("550e8400-e29b-41d4-a716-446655440000")]
        public void Validate_WithValidUserIds_ShouldNotHaveAnyValidationErrors(string userId)
        {
            // Arrange
            var command = new DeleteCommand(userId);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
