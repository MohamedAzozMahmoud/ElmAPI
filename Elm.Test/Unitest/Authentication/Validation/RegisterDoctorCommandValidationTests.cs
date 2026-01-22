using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Validations.Authentication;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Authentication.Validation
{
    public class RegisterDoctorCommandValidationTests
    {
        private readonly RegisterDoctorCommandValidation _validator;

        public RegisterDoctorCommandValidationTests()
        {
            _validator = new RegisterDoctorCommandValidation();
        }

        #region UserName Tests

        [Fact]
        public void Validate_WhenUserNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterDoctorCommand(" ", "Password@123", "Password@123", "Full Name", "Dr.");

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
            var command = new RegisterDoctorCommand("doctor1", "Password@123", "Password@123", "Full Name", "Dr.");

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
            var command = new RegisterDoctorCommand("doctor1", "", "", "Full Name", "Dr.");

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
            var command = new RegisterDoctorCommand("doctor1", "password", "password", "Full Name", "Dr.");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_WhenPasswordIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new RegisterDoctorCommand("doctor1", "Password@123", "Password@123", "Full Name", "Dr.");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        #endregion

        #region ConfirmPassword Tests

        [Fact]
        public void Validate_WhenConfirmPasswordDoesNotMatch_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterDoctorCommand("doctor1", "Password@123", "Different@123", "Full Name", "Dr.");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword)
                .WithErrorMessage("كلمة المرور وتأكيد كلمة المرور غير متطابقين");
        }

        [Fact]
        public void Validate_WhenConfirmPasswordMatches_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new RegisterDoctorCommand("doctor1", "Password@123", "Password@123", "Full Name", "Dr.");

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
            var command = new RegisterDoctorCommand("doctor1", "Password@123", "Password@123", "", "Dr.");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FullName)
                .WithErrorMessage("الاسم الكامل مطلوب");
        }

        [Fact]
        public void Validate_WhenFullNameIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new RegisterDoctorCommand("doctor1", "Password@123", "Password@123", "John Doe", "Dr.");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.FullName);
        }

        #endregion

        #region Title Tests

        [Fact]
        public void Validate_WhenTitleIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterDoctorCommand("doctor1", "Password@123", "Password@123", "Full Name", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("اللقب مطلوب");
        }

        [Fact]
        public void Validate_WhenTitleIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterDoctorCommand("doctor1", "Password@123", "Password@123", "Full Name", null!);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("اللقب مطلوب");
        }

        [Fact]
        public void Validate_WhenTitleIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new RegisterDoctorCommand("doctor1", "Password@123", "Password@123", "Full Name", "Professor");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        #endregion

        #region Combined Tests

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new RegisterDoctorCommand("doctor1", "Password@123", "Password@123", "Dr. John Doe", "Professor");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenAllFieldsAreInvalid_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var command = new RegisterDoctorCommand("", "", "different", "", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName);
            result.ShouldHaveValidationErrorFor(x => x.Password);
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
            result.ShouldHaveValidationErrorFor(x => x.FullName);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Theory]
        [InlineData("Dr.")]
        [InlineData("Professor")]
        [InlineData("Associate Professor")]
        public void Validate_WithValidTitles_ShouldNotHaveValidationError(string title)
        {
            // Arrange
            var command = new RegisterDoctorCommand("doctor1", "Password@123", "Password@123", "Full Name", title);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        #endregion
    }
}
