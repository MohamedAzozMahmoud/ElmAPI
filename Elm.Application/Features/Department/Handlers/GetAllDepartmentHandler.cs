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
        public GetAllDepartmentHandler(IDepartmentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result<List<GetDepartmentDto>>> Handle(GetAllDepartmentQuery request, CancellationToken cancellationToken)
        {
            var departmentDtos = await repository.GetAllDepartmentInYearAsync(request.yearId);

            return Result<List<GetDepartmentDto>>.Success(departmentDtos);
        }
    }
}
