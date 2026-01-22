namespace Elm.Application.Contracts.Features.Questions.DTOs
{
    public record TemplateQuestionsDto
    {
        public string Content { get; set; }
        public string QuestionType { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectOption { get; set; }
    }
}
