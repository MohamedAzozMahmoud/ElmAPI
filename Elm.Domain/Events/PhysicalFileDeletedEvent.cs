using MediatR;

namespace Elm.Domain.Events
{
    public record PhysicalFileDeletedEvent(string FilePath) : INotification;
}