namespace Elm.Domain.Entities
{
    public class College
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Navigation Properties
        public int UniversityId { get; set; }
        public University University { get; set; }
        public int? ImgId { get; set; }
        public Image? Img { get; set; }
        public ICollection<Department> Departments { get; set; }
        public ICollection<Year> Years { get; set; }


    }
}
