namespace Elm.Domain.Entities
{
    public class University
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int? ImgId { get; set; }
        public Image Img { get; set; }
        public ICollection<College> Colleges { get; set; } = new HashSet<College>();
    }
}
