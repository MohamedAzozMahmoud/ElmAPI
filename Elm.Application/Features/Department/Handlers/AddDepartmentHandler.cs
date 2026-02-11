using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.Commands;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Mapper.Elm.Application.Mappers;
using MediatR;

namespace Elm.Application.Features.Department.Handlers
{
    public sealed class AddDepartmentHandler : IRequestHandler<AddDepartmentCommand, Result<DepartmentDto>>
    {
        private readonly IDepartmentRepository repository;
        private readonly MappingProvider _mapping;

        public AddDepartmentHandler(IDepartmentRepository repository, MappingProvider mapping)
        {
            this.repository = repository;
            this._mapping = mapping;
        }
        public async Task<Result<DepartmentDto>> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = new Elm.Domain.Entities.Department
            {
                Name = request.Name,
                IsPaid = request.IsPaid,
                CollegeId = request.collegeId
            };
            var addedDepartment = await repository.AddAsync(department);
            var departmentDto = _mapping.MapToDto(addedDepartment);
            return Result<DepartmentDto>.Success(departmentDto);
        }
    }
}
