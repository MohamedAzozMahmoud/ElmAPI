using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.Commands;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Department.Handlers
{
    public sealed class AddDepartmentHandler : IRequestHandler<AddDepartmentCommand, Result<DepartmentDto>>
    {
        private readonly IDepartmentRepository repository;
        private readonly IMapper mapper;

        public AddDepartmentHandler(IDepartmentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
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
            var departmentDto = mapper.Map<DepartmentDto>(addedDepartment);
            return Result<DepartmentDto>.Success(departmentDto);
        }
    }
}
