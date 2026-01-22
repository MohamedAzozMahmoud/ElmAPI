using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Features.Authentication.DTOs;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Authentication
{
    public class RegisterCommandValidation : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidation()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("اسم المستخدم مطلوب")
                .MaximumLength(50).WithMessage("اسم المستخدم لا يجب أن يتجاوز 50 حرفًا");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة")
                 .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$")
                .WithMessage("يجب ان تكون كلمة المرور الحالية مكونة من 6 احرف على الاقل وتحتوي على حرف كبير واحد على الاقل، وحرف صغير واحد على الاقل، ورقم واحد على الاقل، ورمز خاص واحد على الاقل.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("كلمة المرور وتأكيد كلمة المرور غير متطابقين");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("الاسم الكامل مطلوب")
                .MaximumLength(100).WithMessage("الاسم الكامل لا يجب أن يتجاوز 100 حرفًا");
        }
    }
}
