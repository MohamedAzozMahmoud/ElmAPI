using Elm.Application.Contracts.Features.Authentication.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Authentication
{
    public sealed class ResetPasswordCommandValidation : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidation()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("اسم المستخدم مطلوب");
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("الرمز مطلوب");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("كلمة المرور الجديدة مطلوبة")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$")
                .WithMessage("يجب ان تكون كلمة المرور الجديدة مكونة من 6 احرف على الاقل وتحتوي على حرف كبير واحد على الاقل، وحرف صغير واحد على الاقل، ورقم واحد على الاقل، ورمز خاص واحد على الاقل.");
        }
    }
}
