using Elm.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Elm.Application.Handler
{
    public class PhysicalFileDeletedHandler : INotificationHandler<PhysicalFileDeletedEvent>
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PhysicalFileDeletedHandler(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task Handle(PhysicalFileDeletedEvent notification, CancellationToken cancellationToken)
        {
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, notification.FilePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            return Task.CompletedTask;
        }
    }
}
