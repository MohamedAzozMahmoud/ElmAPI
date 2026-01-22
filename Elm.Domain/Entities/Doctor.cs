namespace Elm.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser User { get; set; }
        public ICollection<Curriculum>? Curriculums { get; set; }
        public ICollection<Files>? RatedFiles { get; set; }

    }
}
