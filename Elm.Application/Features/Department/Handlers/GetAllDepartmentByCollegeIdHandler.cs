using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Features.Department.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Department.Handlers
{
    public sealed class GetAllDepartmentByCollegeIdHandler : IRequestHandler<GetAllDepartmentByCollegeIdQuery, Result<List<GetDepartmentDto>>>
    {
        private readonly IDepartmentRepository repository;
        public GetAllDepartmentByCollegeIdHandler(IDepartmentRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<List<GetDepartmentDto>>> Handle(GetAllDepartmentByCollegeIdQuery request, CancellationToken cancellationToken)
        {
            var departmentDtos = await repository.GetAllDepartmentsByCollegeIdAsync(request.collegeId, request.pageNumber, request.pageSize);

            return Result<List<GetDepartmentDto>>.Success(departmentDtos);
        }
    }
}
