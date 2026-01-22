using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Features.Department.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Department.Handlers
{
    public sealed class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdQuery, Result<GetDepartmentDto>>
    {
        private readonly IDepartmentRepository repository;
        private readonly IMapper mapper;
        public GetDepartmentByIdHandler(IDepartmentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<GetDepartmentDto>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await repository.GetByIdAsync(request.Id);
            if (department == null)
            {
                return Result<GetDepartmentDto>.Failure("Department not found.");
            }
            var departmentDto = mapper.Map<GetDepartmentDto>(department);
            return Result<GetDepartmentDto>.Success(departmentDto);
        }
    }
}
