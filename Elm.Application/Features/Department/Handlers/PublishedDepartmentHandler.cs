using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Department.Handlers
{
    public sealed class PublishedDepartmentHandler : IRequestHandler<PublishedDepartmentCommand, Result<bool>>
    {
        private readonly IDepartmentRepository repository;
        public PublishedDepartmentHandler(IDepartmentRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<bool>> Handle(PublishedDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await repository.GetByIdAsync(request.Id);
            if (department == null)
            {
                return Result<bool>.Failure("لا يوجد قسم بهذا المعرف", 404);
            }
            department.IsPublished = !department.IsPublished;
            var result = await repository.UpdateAsync(department);
            if (!result)
            {
                return Result<bool>.Failure("فشل في تحديث القسم", 500);
            }
            return Result<bool>.Success(true);
        }
    }
}
