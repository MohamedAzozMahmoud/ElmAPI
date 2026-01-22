namespace Elm.Domain.Entities
{
    public class QuestionsBank
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; }
        public ICollection<Question> Questions { get; set; } = new HashSet<Question>();
    }
}
