namespace Elm.Domain.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StorageName { get; set; }
        public string ContentType { get; set; }
        public University? University { get; set; }
        public College? College { get; set; }
    }
}
