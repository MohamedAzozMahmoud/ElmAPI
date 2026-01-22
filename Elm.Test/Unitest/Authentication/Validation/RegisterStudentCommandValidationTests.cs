using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Validations.Authentication;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Authentication.Validation
{
    public class RegisterStudentCommandValidationTests
    {
        private readonly RegisterStudentCommandValidation _validator;

        public RegisterStudentCommandValidationTests()
        {
            _validator = new RegisterStudentCommandValidation();
        }

        #region UserName Tests

        [Fact]
        public void Validate_WhenUserNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterStudentCommand(" ", "Password@123", "Password@123", "Full Name", 1, 1);

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
            var command = new RegisterStudentCommand("student1", "Password@123", "Password@123", "Full Name", 1, 1);

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
            var command = new RegisterStudentCommand("student1", "", "", "Full Name", 1, 1);

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
            var command = new RegisterStudentCommand("student1", "password", "password", "Full Name", 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_WhenPasswordIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new RegisterStudentCommand("student1", "Password@123", "Password@123", "Full Name", 1, 1);

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
            var command = new RegisterStudentCommand("student1", "Password@123", "Different@123", "Full Name", 1, 1);

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
            var command = new RegisterStudentCommand("student1", "Password@123", "Password@123", "Full Name", 1, 1);

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
            var command = new RegisterStudentCommand("student1", "Password@123", "Password@123", "", 1, 1);

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
            var command = new RegisterStudentCommand("student1", "Password@123", "Password@123", "John Doe", 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.FullName);
        }

        #endregion

        #region DepartmentId Tests

        [Fact]
        public void Validate_WhenDepartmentIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterStudentCommand("student1", "Password@123", "Password@123", "Full Name", 0, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DepartmentId)
                .WithErrorMessage("القسم مطلوب");
        }

        [Fact]
        public void Validate_WhenDepartmentIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterStudentCommand("student1", "Password@123", "Password@123", "Full Name", -1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DepartmentId)
                .WithErrorMessage("القسم مطلوب");
        }

        [Fact]
        public void Validate_WhenDepartmentIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new RegisterStudentCommand("student1", "Password@123", "Password@123", "Full Name", 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.DepartmentId);
        }

        #endregion

        #region YearId Tests

        [Fact]
        public void Validate_WhenYearIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterStudentCommand("student1", "Password@123", "Password@123", "Full Name", 1, 0);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.YearId)
                .WithErrorMessage("السنة الدراسية مطلوبة");
        }

        [Fact]
        public void Validate_WhenYearIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var command = new RegisterStudentCommand("student1", "Password@123", "Password@123", "Full Name", 1, -1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.YearId)
                .WithErrorMessage("السنة الدراسية مطلوبة");
        }

        [Fact]
        public void Validate_WhenYearIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new RegisterStudentCommand("student1", "Password@123", "Password@123", "Full Name", 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.YearId);
        }

        #endregion

        #region Combined Tests

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new RegisterStudentCommand("student1", "Password@123", "Password@123", "John Doe", 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenAllFieldsAreInvalid_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var command = new RegisterStudentCommand("", "", "different", "", 0, 0);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName);
            result.ShouldHaveValidationErrorFor(x => x.Password);
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
            result.ShouldHaveValidationErrorFor(x => x.FullName);
            result.ShouldHaveValidationErrorFor(x => x.DepartmentId);
            result.ShouldHaveValidationErrorFor(x => x.YearId);
        }

        #endregion
    }
}
