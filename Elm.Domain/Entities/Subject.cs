namespace Elm.Domain.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public ICollection<Curriculum> Curriculums { get; set; } = new HashSet<Curriculum>();

    }
}
