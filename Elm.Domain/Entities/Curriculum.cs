namespace Elm.Domain.Entities
{
    public class Curriculum
    {
        public int Id { get; set; }
        // Navigation Properties
        public int YearId { get; set; }
        public Year Year { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public ICollection<QuestionsBank> QuestionsBanks { get; set; } = new HashSet<QuestionsBank>();
        public ICollection<Files> Files { get; set; } = new HashSet<Files>();
    }
}
