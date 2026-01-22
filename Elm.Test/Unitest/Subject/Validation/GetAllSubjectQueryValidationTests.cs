using Elm.Application.Contracts.Features.Subject.Queries;
using Elm.Application.Contracts.Validations.Subject;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Subject.Validation
{
    public class GetAllSubjectQueryValidationTests
    {
        private readonly GetAllSubjectQueryValidation _validator;

        public GetAllSubjectQueryValidationTests()
        {
            _validator = new GetAllSubjectQueryValidation();
        }

        [Fact]
        public void Validate_WhenDepartmentIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var query = new GetAllSubjectQuery(0);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.departmentId)
                .WithErrorMessage("Department ID must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenDepartmentIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var query = new GetAllSubjectQuery(-1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.departmentId)
                .WithErrorMessage("Department ID must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenDepartmentIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var query = new GetAllSubjectQuery(1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.departmentId);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void Validate_WithValidPositiveDepartmentIds_ShouldNotHaveAnyValidationErrors(int departmentId)
        {
            // Arrange
            var query = new GetAllSubjectQuery(departmentId);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(-1000)]
        public void Validate_WithInvalidDepartmentIds_ShouldHaveValidationError(int departmentId)
        {
            // Arrange
            var query = new GetAllSubjectQuery(departmentId);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.departmentId)
                .WithErrorMessage("Department ID must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenDepartmentIdIsMaxInt_ShouldNotHaveValidationError()
        {
            // Arrange
            var query = new GetAllSubjectQuery(int.MaxValue);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
