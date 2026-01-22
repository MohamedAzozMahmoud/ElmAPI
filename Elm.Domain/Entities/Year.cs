namespace Elm.Domain.Entities
{
    public class Year
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int CollegeId { get; set; }
        public College College { get; set; }

        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
