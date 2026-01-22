using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.Commands;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Department.Handlers
{
    public sealed class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, Result<DepartmentDto>>
    {
        private readonly IDepartmentRepository repository;
        private readonly IMapper mapper;
        public UpdateDepartmentHandler(IDepartmentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<DepartmentDto>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await repository.GetByIdAsync(request.Id);
            if (department == null)
            {
                return Result<DepartmentDto>.Failure("Department not found");
            }
            department.Name = request.Name;
            department.IsPaid = request.IsPaid;
            var updatedDepartment = await repository.UpdateAsync(department);
            var departmentDto = mapper.Map<DepartmentDto>(updatedDepartment);
            return Result<DepartmentDto>.Success(departmentDto);
        }
    }
}
