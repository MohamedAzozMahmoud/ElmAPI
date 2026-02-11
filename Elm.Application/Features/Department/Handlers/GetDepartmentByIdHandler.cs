using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Features.Department.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Mapper.Elm.Application.Mappers;
using MediatR;

namespace Elm.Application.Features.Department.Handlers
{
    public sealed class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdQuery, Result<GetDepartmentDto>>
    {
        private readonly IDepartmentRepository repository;
        private readonly MappingProvider _mapping;
        public GetDepartmentByIdHandler(IDepartmentRepository repository, MappingProvider mapping)
        {
            this.repository = repository;
            this._mapping = mapping;
        }
        public async Task<Result<GetDepartmentDto>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await repository.GetByIdAsync(request.Id);
            if (department == null)
            {
                return Result<GetDepartmentDto>.Failure("Department not found.");
            }
            var departmentDto = _mapping.MapToGetDto(department);
            return Result<GetDepartmentDto>.Success(departmentDto);
        }
    }
}
