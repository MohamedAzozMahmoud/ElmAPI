using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Subject.Handlers
{
    public sealed class DeleteSubjectHandler : IRequestHandler<DeleteSubjectCommand, Result<bool>>
    {
        private readonly ISubjectRepository repository;
        public DeleteSubjectHandler(ISubjectRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<bool>> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            var end = await repository.DeleteAsync(request.Id);
            if (!end)
            {
                return Result<bool>.Failure("Failed to delete subject.");
            }
            return Result<bool>.Success(true);
        }
    }
}
