using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Validations.Authentication;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Authentication.Validation
{
    public class ResetPasswordCommandValidationTests
    {
        private readonly ResetPasswordCommandValidation _validator;

        public ResetPasswordCommandValidationTests()
        {
            _validator = new ResetPasswordCommandValidation();
        }

        #region UserName Tests

        [Fact]
        public void Validate_WhenUserNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new ResetPasswordCommand(" ", "token123", "NewPass@123");

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
            var command = new ResetPasswordCommand(null!, "token123", "NewPass@123");

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
            var command = new ResetPasswordCommand("testuser", "token123", "NewPass@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }

        #endregion

        #region Token Tests

        [Fact]
        public void Validate_WhenTokenIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", "", "NewPass@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Token)
                .WithErrorMessage("الرمز مطلوب");
        }

        [Fact]
        public void Validate_WhenTokenIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", null!, "NewPass@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Token)
                .WithErrorMessage("الرمز مطلوب");
        }

        [Fact]
        public void Validate_WhenTokenIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", "valid-token-123", "NewPass@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Token);
        }

        #endregion

        #region NewPassword Tests

        [Fact]
        public void Validate_WhenNewPasswordIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", "token123", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword)
                .WithErrorMessage("كلمة المرور الجديدة مطلوبة");
        }

        [Fact]
        public void Validate_WhenNewPasswordDoesNotMatchPattern_ShouldHaveValidationError()
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", "token123", "weakpassword");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Validate_WhenNewPasswordWithoutUppercase_ShouldHaveValidationError()
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", "token123", "password@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Validate_WhenNewPasswordWithoutSpecialChar_ShouldHaveValidationError()
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", "token123", "Password123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Validate_WhenNewPasswordIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", "token123", "NewPass@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.NewPassword);
        }

        #endregion

        #region Combined Tests

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", "valid-token", "NewPass@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenAllFieldsAreInvalid_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var command = new ResetPasswordCommand("", "", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName);
            result.ShouldHaveValidationErrorFor(x => x.Token);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Theory]
        [InlineData("Pass@123")]
        [InlineData("SecureP@ss1")]
        [InlineData("MyP@ssw0rd")]
        public void Validate_WithValidPasswords_ShouldNotHaveValidationError(string password)
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", "token123", password);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.NewPassword);
        }

        #endregion
    }
}
