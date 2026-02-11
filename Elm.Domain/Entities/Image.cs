using Elm.Domain.Common;
using Elm.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elm.Domain.Entities
{
    public class Image : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StorageName { get; set; }
        public string ContentType { get; set; }
        public University? University { get; set; }
        public College? College { get; set; }

        [NotMapped]
        public string FilePath => Path.Combine("Images", StorageName);
        public void MarkAsDeleted()
        {
            AddDomainEvent(new PhysicalFileDeletedEvent(this.FilePath));
        }
    }
}
