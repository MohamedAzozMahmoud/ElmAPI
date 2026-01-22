using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Validations.Authentication;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Authentication.Validation
{
    public class RegisterCommandValidationTests
    {
        private readonly RegisterCommandValidation _validator;

        public RegisterCommandValidationTests()
        {
            _validator = new RegisterCommandValidation();
        }

        #region UserName Tests

        [Fact]
        public void Validate_WhenUserNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterCommand(" ", "Password@123", "Password@123", "Full Name");

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
            var command = new RegisterCommand(null, "Password@123", "Password@123", "Full Name");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage("اسم المستخدم مطلوب");
        }

        [Fact]
        public void Validate_WhenUserNameExceeds50Characters_ShouldHaveValidationError()
        {
            // Arrange
            var longUserName = new string('A', 51);
            var command = new RegisterCommand(longUserName, "Password@123", "Password@123", "Full Name");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage("اسم المستخدم لا يجب أن يتجاوز 50 حرفًا");
        }

        [Fact]
        public void Validate_WhenUserNameIs50Characters_ShouldNotHaveValidationError()
        {
            // Arrange
            var exactUserName = new string('A', 50);
            var command = new RegisterCommand(exactUserName, "Password@123", "Password@123", "Full Name");

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
            var command = new RegisterCommand("testuser", " ", " ", "Full Name");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("كلمة المرور مطلوبة");
        }

        [Fact]
        public void Validate_WhenPasswordDoesNotMatchPattern_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterCommand("testuser", "password", "password", "Full Name");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_WhenPasswordIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new RegisterCommand("testuser", "Password@123", "Password@123", "Full Name");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        #endregion

        #region ConfirmPassword Tests

        [Fact]
        public void Validate_WhenConfirmPasswordDoesNotMatchPassword_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterCommand("testuser", "Password@123", "DifferentPassword@123", "Full Name");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword)
                .WithErrorMessage("كلمة المرور وتأكيد كلمة المرور غير متطابقين");
        }

        [Fact]
        public void Validate_WhenConfirmPasswordMatchesPassword_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new RegisterCommand("testuser", "Password@123", "Password@123", "Full Name");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ConfirmPassword);
        }

        #endregion

        #region FullName Tests

        [Fact]
        public void Validate_WhenFullNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterCommand("testuser", "Password@123", "Password@123", " ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FullName)
                .WithErrorMessage("الاسم الكامل مطلوب");
        }

        [Fact]
        public void Validate_WhenFullNameExceeds100Characters_ShouldHaveValidationError()
        {
            // Arrange
            var longFullName = new string('A', 101);
            var command = new RegisterCommand("testuser", "Password@123", "Password@123", longFullName);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FullName)
                .WithErrorMessage("الاسم الكامل لا يجب أن يتجاوز 100 حرفًا");
        }

        [Fact]
        public void Validate_WhenFullNameIs100Characters_ShouldNotHaveValidationError()
        {
            // Arrange
            var exactFullName = new string('A', 100);
            var command = new RegisterCommand("testuser", "Password@123", "Password@123", exactFullName);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.FullName);
        }

        #endregion

        #region Combined Tests

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new RegisterCommand("testuser", "Password@123", "Password@123", "John Doe");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenAllFieldsAreInvalid_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var command = new RegisterCommand(" ", " ", "different", " ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName);
            result.ShouldHaveValidationErrorFor(x => x.Password);
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
            result.ShouldHaveValidationErrorFor(x => x.FullName);
        }

        #endregion
    }
}
