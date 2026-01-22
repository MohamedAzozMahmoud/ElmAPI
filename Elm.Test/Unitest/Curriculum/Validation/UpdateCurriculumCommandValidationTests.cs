using Elm.Application.Contracts.Features.Curriculum.Commands;
using Elm.Application.Contracts.Validations.Curriculum;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Curriculum.Validation
{
    public class UpdateCurriculumCommandValidationTests
    {
        private readonly UpdateCurriculumCommandValidation _validator;

        public UpdateCurriculumCommandValidationTests()
        {
            _validator = new UpdateCurriculumCommandValidation();
        }

        #region Id Tests

        [Fact]
        public void Validate_WhenIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(0, 1, 1, 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(-1, 1, 1, 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 1, 1, 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        #endregion

        #region SubjectId Tests

        [Fact]
        public void Validate_WhenSubjectIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 0, 1, 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubjectId)
                .WithErrorMessage("SubjectId must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenSubjectIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, -1, 1, 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubjectId)
                .WithErrorMessage("SubjectId must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenSubjectIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 1, 1, 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.SubjectId);
        }

        #endregion

        #region YearId Tests

        [Fact]
        public void Validate_WhenYearIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 1, 0, 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.YearId)
                .WithErrorMessage("YearId must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenYearIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 1, -1, 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.YearId)
                .WithErrorMessage("YearId must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenYearIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 1, 1, 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.YearId);
        }

        #endregion

        #region DepartmentId Tests

        [Fact]
        public void Validate_WhenDepartmentIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 1, 1, 0, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DepartmentId)
                .WithErrorMessage("DepartmentId must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenDepartmentIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 1, 1, -1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DepartmentId)
                .WithErrorMessage("DepartmentId must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenDepartmentIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 1, 1, 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.DepartmentId);
        }

        #endregion

        #region DoctorId Tests

        [Fact]
        public void Validate_WhenDoctorIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 1, 1, 1, 0);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DoctorId)
                .WithErrorMessage("DoctorId must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenDoctorIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 1, 1, 1, -1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DoctorId)
                .WithErrorMessage("DoctorId must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenDoctorIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 1, 1, 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.DoctorId);
        }

        #endregion

        #region Combined Tests

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(1, 2, 3, 4, 5);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(1, 1, 1, 1, 1)]
        [InlineData(10, 20, 30, 40, 50)]
        [InlineData(100, 200, 300, 400, 500)]
        public void Validate_WithValidInputs_ShouldNotHaveAnyValidationErrors(int id, int subjectId, int yearId, int departmentId, int doctorId)
        {
            // Arrange
            var command = new UpdateCurriculumCommand(id, subjectId, yearId, departmentId, doctorId);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenAllFieldsAreZero_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(0, 0, 0, 0, 0);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.SubjectId);
            result.ShouldHaveValidationErrorFor(x => x.YearId);
            result.ShouldHaveValidationErrorFor(x => x.DepartmentId);
            result.ShouldHaveValidationErrorFor(x => x.DoctorId);
        }

        [Fact]
        public void Validate_WhenAllFieldsAreNegative_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(-1, -2, -3, -4, -5);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.SubjectId);
            result.ShouldHaveValidationErrorFor(x => x.YearId);
            result.ShouldHaveValidationErrorFor(x => x.DepartmentId);
            result.ShouldHaveValidationErrorFor(x => x.DoctorId);
        }

        [Fact]
        public void Validate_WhenOnlyIdIsInvalid_ShouldHaveOnlyIdValidationError()
        {
            // Arrange
            var command = new UpdateCurriculumCommand(0, 1, 1, 1, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldNotHaveValidationErrorFor(x => x.SubjectId);
            result.ShouldNotHaveValidationErrorFor(x => x.YearId);
            result.ShouldNotHaveValidationErrorFor(x => x.DepartmentId);
            result.ShouldNotHaveValidationErrorFor(x => x.DoctorId);
        }

        #endregion
    }
}
