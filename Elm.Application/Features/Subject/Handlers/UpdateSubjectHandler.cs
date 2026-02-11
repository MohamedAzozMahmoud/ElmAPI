using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Subject.Handlers
{
    public sealed class UpdateSubjectHandler : IRequestHandler<UpdateSubjectCommand, Result<bool>>
    {
        private readonly ISubjectRepository repository;
        public UpdateSubjectHandler(ISubjectRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<bool>> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject = await repository.GetByIdAsync(request.Id);
            if (subject is null)
            {
                return Result<bool>.Failure("Subject not found.");
            }
            subject.Name = request.Name;
            subject.Code = request.Code;
            var result = await repository.UpdateAsync(subject);
            if (!result)
            {
                return Result<bool>.Failure("Failed to update subject.");
            }
            return Result<bool>.Success(result);
        }
    }
}
