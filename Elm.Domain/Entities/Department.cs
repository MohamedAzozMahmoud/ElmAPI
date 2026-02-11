using Elm.Domain.Enums;

namespace Elm.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsPaid { get; set; }
        public TypeOfDepartment Type { get; set; } = TypeOfDepartment.General;
        public bool IsPublished { get; set; } = true; // تحكم يدوي للأدمن (Force Hide)
        // Navigation Properties
        public ICollection<Curriculum> Curriculums { get; set; } = new HashSet<Curriculum>();
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
        public int CollegeId { get; set; }
        public College College { get; set; }
    }
}
