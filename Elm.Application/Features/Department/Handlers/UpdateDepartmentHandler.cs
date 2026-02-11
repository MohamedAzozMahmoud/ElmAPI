using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Department.Handlers
{
    public sealed class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, Result<bool>>
    {
        private readonly IDepartmentRepository repository;
        public UpdateDepartmentHandler(IDepartmentRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<bool>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await repository.GetByIdAsync(request.Id);
            if (department == null)
            {
                return Result<bool>.Failure("لا يوجد قسم بهذا المعرف", 404);
            }
            department.Name = request.Name;
            department.IsPaid = request.IsPaid;
            department.Type = request.Type;
            var result = await repository.UpdateAsync(department);
            if (!result)
            {
                return Result<bool>.Failure("فشل في تحديث القسم", 500);
            }
            return Result<bool>.Success(true);
        }
    }
}
