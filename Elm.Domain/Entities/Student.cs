namespace Elm.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public Year Year { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public string AppUserId { get; set; }
        public AppUser? User { get; set; }
        public ICollection<Files> UploadedFiles { get; set; } = new HashSet<Files>();
    }
}
