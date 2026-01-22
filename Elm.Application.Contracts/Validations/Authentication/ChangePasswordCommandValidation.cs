using Elm.Application.Contracts.Features.Authentication.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Authentication
{
    public sealed class ChangePasswordCommandValidation : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidation()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("معرف المستخدم مطلوب");
            RuleFor(x => x.currentPassword)
                .NotEmpty().WithMessage("كلمة المرور الحالية مطلوبة");
            RuleFor(x => x.newPassword)
                .NotEmpty().WithMessage("كلمة المرور الجديدة مطلوبة")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$")
                .WithMessage("يجب ان تكون كلمة المرور الجديدة مكونة من 6 احرف على الاقل وتحتوي على حرف كبير واحد على الاقل، وحرف صغير واحد على الاقل، ورقم واحد على الاقل، ورمز خاص واحد على الاقل.");
            RuleFor(x => x.confidentialPassword)
                .Equal(x => x.newPassword).WithMessage("كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقين");
        }
    }
}
