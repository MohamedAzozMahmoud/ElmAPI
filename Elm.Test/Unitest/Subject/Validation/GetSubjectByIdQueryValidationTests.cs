using Elm.Application.Contracts.Features.Subject.Queries;
using Elm.Application.Contracts.Validations.Subject;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Subject.Validation
{
    public class GetSubjectByIdQueryValidationTests
    {
        private readonly GetSubjectByIdQueryValidation _validator;

        public GetSubjectByIdQueryValidationTests()
        {
            _validator = new GetSubjectByIdQueryValidation();
        }

        [Fact]
        public void Validate_WhenSubjectIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var query = new GetSubjectByIdQuery(0);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubjectId)
                .WithErrorMessage("Subject ID must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenSubjectIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var query = new GetSubjectByIdQuery(-1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubjectId)
                .WithErrorMessage("Subject ID must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenSubjectIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var query = new GetSubjectByIdQuery(1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.SubjectId);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void Validate_WithValidPositiveSubjectIds_ShouldNotHaveAnyValidationErrors(int subjectId)
        {
            // Arrange
            var query = new GetSubjectByIdQuery(subjectId);

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
        public void Validate_WithInvalidSubjectIds_ShouldHaveValidationError(int subjectId)
        {
            // Arrange
            var query = new GetSubjectByIdQuery(subjectId);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubjectId)
                .WithErrorMessage("Subject ID must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenSubjectIdIsMaxInt_ShouldNotHaveValidationError()
        {
            // Arrange
            var query = new GetSubjectByIdQuery(int.MaxValue);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
