using Elm.Application.Contracts.Features.Notifications.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Notifications
{
    public sealed class MarkAllNotificationsAsReadCommandValidation : AbstractValidator<MarkAllNotificationsAsReadCommand>
    {
        public MarkAllNotificationsAsReadCommandValidation()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId must not be empty.");
        }
    }
}
