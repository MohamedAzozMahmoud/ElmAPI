using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elm.Domain.Common
{
    public abstract class BaseEntity
    {
        [NotMapped]
        public List<INotification> _domainEvents = new();
        [NotMapped] // لا تظهر في قاعدة البيانات
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(INotification @event) => _domainEvents.Add(@event);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
