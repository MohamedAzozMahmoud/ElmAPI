using Elm.Application.Contracts.Features.Images.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Images
{
    public sealed class DeleteImageByNameCommandValidation : AbstractValidator<DeleteImageByNameCommand>
    {
        public DeleteImageByNameCommandValidation()
        {
            RuleFor(x => x.fileName)
                .NotEmpty().WithMessage("يجب إدخال اسم الملف.");
        }
    }
}
