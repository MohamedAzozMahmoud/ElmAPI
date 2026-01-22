using Elm.Application.Contracts.Features.Options.DTOs;
using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Validations.Questions;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Questions.Validation
{
    public class AddQuestionCommandValidationTests
    {
        private readonly AddQuestionCommandValidation _validator;

        public AddQuestionCommandValidationTests()
        {
            _validator = new AddQuestionCommandValidation();
        }

        #region QuestionBankId Validation Tests

        [Fact]
        public void Validate_WhenQuestionBankIdIsZero_ShouldHaveError()
        {
            // Arrange
            var questionsDto = CreateValidAddQuestionsDto();
            var command = new AddQuestionCommand(0, questionsDto);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.questionBankId)
                .WithErrorMessage("Question Bank Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenQuestionBankIdIsNegative_ShouldHaveError()
        {
            // Arrange
            var questionsDto = CreateValidAddQuestionsDto();
            var command = new AddQuestionCommand(-1, questionsDto);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.questionBankId)
                .WithErrorMessage("Question Bank Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenQuestionBankIdIsPositive_ShouldNotHaveError()
        {
            // Arrange
            var questionsDto = CreateValidAddQuestionsDto();
            var command = new AddQuestionCommand(1, questionsDto);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.questionBankId);
        }

        #endregion

        #region QuestionsDto Validation Tests

        [Fact]
        public void Validate_WhenQuestionsDtoContentIsEmpty_ShouldHaveError()
        {
            // Arrange
            var questionsDto = new AddQuestionsDto
            {
                Content = "",
                QuestionType = "MultipleChoice",
                Options = new List<AddOptionsDto>
                {
                    new AddOptionsDto { Content = "Option 1", IsCorrect = true }
                }
            };
            var command = new AddQuestionCommand(1, questionsDto);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.QuestionsDto.Content);
        }

        [Fact]
        public void Validate_WhenQuestionsDtoQuestionTypeIsInvalid_ShouldHaveError()
        {
            // Arrange
            var questionsDto = new AddQuestionsDto
            {
                Content = "Valid content",
                QuestionType = "InvalidType",
                Options = new List<AddOptionsDto>
                {
                    new AddOptionsDto { Content = "Option 1", IsCorrect = true }
                }
            };
            var command = new AddQuestionCommand(1, questionsDto);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.QuestionsDto.QuestionType);
        }

        [Theory]
        [InlineData("MultipleChoice")]
        [InlineData("TrueFalse")]
        [InlineData("ShortAnswer")]
        public void Validate_WhenQuestionsDtoQuestionTypeIsValid_ShouldNotHaveError(string questionType)
        {
            // Arrange
            var questionsDto = new AddQuestionsDto
            {
                Content = "Valid content",
                QuestionType = questionType,
                Options = new List<AddOptionsDto>
                {
                    new AddOptionsDto { Content = "Option 1", IsCorrect = true }
                }
            };
            var command = new AddQuestionCommand(1, questionsDto);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.QuestionsDto.QuestionType);
        }

        #endregion

        #region Options Validation Tests

        [Fact]
        public void Validate_WhenOptionContentIsEmpty_ShouldHaveError()
        {
            // Arrange
            var questionsDto = new AddQuestionsDto
            {
                Content = "Valid content",
                QuestionType = "MultipleChoice",
                Options = new List<AddOptionsDto>
                {
                    new AddOptionsDto { Content = "", IsCorrect = true }
                }
            };
            var command = new AddQuestionCommand(1, questionsDto);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor("QuestionsDto.Options[0].Content");
        }

        [Fact]
        public void Validate_WhenOptionContentExceeds500Characters_ShouldHaveError()
        {
            // Arrange
            var longContent = new string('a', 501);
            var questionsDto = new AddQuestionsDto
            {
                Content = "Valid content",
                QuestionType = "MultipleChoice",
                Options = new List<AddOptionsDto>
                {
                    new AddOptionsDto { Content = longContent, IsCorrect = true }
                }
            };
            var command = new AddQuestionCommand(1, questionsDto);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor("QuestionsDto.Options[0].Content");
        }

        [Fact]
        public void Validate_WhenOptionsAreValid_ShouldNotHaveError()
        {
            // Arrange
            var questionsDto = new AddQuestionsDto
            {
                Content = "Valid content",
                QuestionType = "MultipleChoice",
                Options = new List<AddOptionsDto>
                {
                    new AddOptionsDto { Content = "Option 1", IsCorrect = true },
                    new AddOptionsDto { Content = "Option 2", IsCorrect = false }
                }
            };
            var command = new AddQuestionCommand(1, questionsDto);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor("QuestionsDto.Options[0].Content");
            result.ShouldNotHaveValidationErrorFor("QuestionsDto.Options[1].Content");
        }

        #endregion

        #region Full Command Validation Tests

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyErrors()
        {
            // Arrange
            var questionsDto = CreateValidAddQuestionsDto();
            var command = new AddQuestionCommand(1, questionsDto);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        #endregion

        #region Helper Methods

        private static AddQuestionsDto CreateValidAddQuestionsDto()
        {
            return new AddQuestionsDto
            {
                Content = "What is 2 + 2?",
                QuestionType = "MultipleChoice",
                Options = new List<AddOptionsDto>
                {
                    new AddOptionsDto { Content = "3", IsCorrect = false },
                    new AddOptionsDto { Content = "4", IsCorrect = true },
                    new AddOptionsDto { Content = "5", IsCorrect = false }
                }
            };
        }

        #endregion
    }
}
