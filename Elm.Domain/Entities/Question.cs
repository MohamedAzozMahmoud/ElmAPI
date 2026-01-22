using Elm.Domain.Enums;

namespace Elm.Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public QuestionType QuestionType { get; set; } = QuestionType.MCQ;
        public int QuestionBankId { get; set; }
        public QuestionsBank QuestionBank { get; set; }
        public ICollection<Option> Options { get; set; } = new HashSet<Option>();
    }
}
