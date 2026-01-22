using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Features.Department.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Department.Handlers
{
    public sealed class GetAllDepartmentHandler : IRequestHandler<GetAllDepartmentQuery, Result<List<GetDepartmentDto>>>
    {
        private readonly IDepartmentRepository repository;
        private readonly IMapper mapper;
        public GetAllDepartmentHandler(IDepartmentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<List<GetDepartmentDto>>> Handle(GetAllDepartmentQuery request, CancellationToken cancellationToken)
        {
            var departments = await repository.GetAllDepartmentInCollegeAsync(request.yearId);
            var departmentDtos = mapper.Map<List<GetDepartmentDto>>(departments);
            return Result<List<GetDepartmentDto>>.Success(departmentDtos);
        }
    }
}
