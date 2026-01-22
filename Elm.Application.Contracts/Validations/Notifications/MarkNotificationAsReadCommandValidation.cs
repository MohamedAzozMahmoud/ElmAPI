using Elm.Application.Contracts.Features.Notifications.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Notifications
{
    public sealed class MarkNotificationAsReadCommandValidation : AbstractValidator<MarkNotificationAsReadCommand>
    {
        public MarkNotificationAsReadCommandValidation()
        {
            RuleFor(x => x.NotificationId)
                .GreaterThan(0).WithMessage("NotificationId must be greater than zero.");
        }
    }
}
