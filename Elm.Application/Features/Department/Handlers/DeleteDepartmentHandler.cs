using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Department.Handlers
{
    public sealed class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, Result<bool>>
    {
        private readonly IDepartmentRepository repository;
        public DeleteDepartmentHandler(IDepartmentRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<bool>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var result = await repository.DeleteAsync(request.Id);
            if (!result)
            {
                return Result<bool>.Failure("Failed to delete department", 500);
            }
            return Result<bool>.Success(true);
        }
    }
}
