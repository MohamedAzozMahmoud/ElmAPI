using Elm.Application.Contracts.Features.Authentication.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Authentication
{
    public sealed class DeleteCommandValidation : AbstractValidator<DeleteCommand>
    {
        public DeleteCommandValidation()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("معرف المستخدم مطلوب");
        }
    }
}
