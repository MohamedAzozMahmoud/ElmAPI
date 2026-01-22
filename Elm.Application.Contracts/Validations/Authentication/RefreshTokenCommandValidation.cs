using Elm.Application.Contracts.Features.Authentication.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Authentication
{
    public sealed class RefreshTokenCommandValidation : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidation()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("الرمز المميز مطلوب");
        }
    }
}
