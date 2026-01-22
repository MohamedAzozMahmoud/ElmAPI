using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Validations.Authentication;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Authentication.Validation
{
    public class LoginCommandValidationTests
    {
        private readonly LoginCommandValidation _validator;

        public LoginCommandValidationTests()
        {
            _validator = new LoginCommandValidation();
        }

        #region UserName Tests

        [Fact]
        public void Validate_WhenUserNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand("", "Password@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage("اسم المستخدم مطلوب");
        }

        [Fact]
        public void Validate_WhenUserNameIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand(null!, "Password@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage("اسم المستخدم مطلوب");
        }

        [Fact]
        public void Validate_WhenUserNameIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand("testuser", "Password@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }

        #endregion

        #region Password Tests

        [Fact]
        public void Validate_WhenPasswordIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand("testuser", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("كلمة المرور مطلوبة");
        }

        [Fact]
        public void Validate_WhenPasswordIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand("testuser", null!);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("كلمة المرور مطلوبة");
        }

        [Fact]
        public void Validate_WhenPasswordDoesNotMatchPattern_ShouldHaveValidationError()
        {
            // Arrange - Password without special character
            var command = new LoginCommand("testuser", "Password123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_WhenPasswordIsTooShort_ShouldHaveValidationError()
        {
            // Arrange - Password less than 6 characters
            var command = new LoginCommand("testuser", "Pa@1");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_WhenPasswordWithoutUppercase_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand("testuser", "password@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_WhenPasswordWithoutLowercase_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand("testuser", "PASSWORD@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_WhenPasswordWithoutDigit_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand("testuser", "Password@abc");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_WhenPasswordIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand("testuser", "Password@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        #endregion

        #region Combined Tests

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new LoginCommand("testuser", "Password@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("user1", "Pass@123")]
        [InlineData("admin", "Admin@456")]
        [InlineData("doctor", "Doctor@789")]
        public void Validate_WithValidInputs_ShouldNotHaveAnyValidationErrors(string userName, string password)
        {
            // Arrange
            var command = new LoginCommand(userName, password);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenAllFieldsAreInvalid_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var command = new LoginCommand("", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        #endregion
    }
}
