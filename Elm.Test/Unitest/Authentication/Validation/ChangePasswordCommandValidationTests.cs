using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Validations.Authentication;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Authentication.Validation
{
    public class ChangePasswordCommandValidationTests
    {
        private readonly ChangePasswordCommandValidation _validator;

        public ChangePasswordCommandValidationTests()
        {
            _validator = new ChangePasswordCommandValidation();
        }

        #region UserId Tests

        [Fact]
        public void Validate_WhenUserIdIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new ChangePasswordCommand(" ", "OldPass@123", "NewPass@123", "NewPass@123");

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
            var command = new ChangePasswordCommand(null!, "OldPass@123", "NewPass@123", "NewPass@123");

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
            var command = new ChangePasswordCommand("user-123", "OldPass@123", "NewPass@123", "NewPass@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }

        #endregion

        #region CurrentPassword Tests

        [Fact]
        public void Validate_WhenCurrentPasswordIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new ChangePasswordCommand("user-123", "", "NewPass@123", "NewPass@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.currentPassword)
                .WithErrorMessage("كلمة المرور الحالية مطلوبة");
        }

        [Fact]
        public void Validate_WhenCurrentPasswordIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new ChangePasswordCommand("user-123", "OldPass@123", "NewPass@123", "NewPass@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.currentPassword);
        }

        #endregion

        #region NewPassword Tests

        [Fact]
        public void Validate_WhenNewPasswordIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new ChangePasswordCommand("user-123", "OldPass@123", "", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.newPassword)
                .WithErrorMessage("كلمة المرور الجديدة مطلوبة");
        }

        [Fact]
        public void Validate_WhenNewPasswordDoesNotMatchPattern_ShouldHaveValidationError()
        {
            // Arrange
            var command = new ChangePasswordCommand("user-123", "OldPass@123", "weakpass", "weakpass");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.newPassword);
        }

        [Fact]
        public void Validate_WhenNewPasswordIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new ChangePasswordCommand("user-123", "OldPass@123", "NewPass@123", "NewPass@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.newPassword);
        }

        #endregion

        #region ConfidentialPassword Tests

        [Fact]
        public void Validate_WhenConfidentialPasswordDoesNotMatch_ShouldHaveValidationError()
        {
            // Arrange
            var command = new ChangePasswordCommand("user-123", "OldPass@123", "NewPass@123", "Different@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.confidentialPassword)
                .WithErrorMessage("كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقين");
        }

        [Fact]
        public void Validate_WhenConfidentialPasswordMatches_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new ChangePasswordCommand("user-123", "OldPass@123", "NewPass@123", "NewPass@123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.confidentialPassword);
        }

        #endregion

        #region Combined Tests

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new ChangePasswordCommand("user-123", "OldPass@123", "NewPass@456", "NewPass@456");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenAllFieldsAreInvalid_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var command = new ChangePasswordCommand("", "", "", "different");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId);
            result.ShouldHaveValidationErrorFor(x => x.currentPassword);
            result.ShouldHaveValidationErrorFor(x => x.newPassword);
            result.ShouldHaveValidationErrorFor(x => x.confidentialPassword);
        }

        #endregion
    }
}
