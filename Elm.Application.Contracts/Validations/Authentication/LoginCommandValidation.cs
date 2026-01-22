using Elm.Application.Contracts.Features.Authentication.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Authentication
{
    public sealed class LoginCommandValidation : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidation()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("اسم المستخدم مطلوب");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$")
                .WithMessage("يجب ان تكون كلمة المرور الحالية مكونة من 6 احرف على الاقل وتحتوي على حرف كبير واحد على الاقل، وحرف صغير واحد على الاقل، ورقم واحد على الاقل، ورمز خاص واحد على الاقل.");
        }
    }
}
