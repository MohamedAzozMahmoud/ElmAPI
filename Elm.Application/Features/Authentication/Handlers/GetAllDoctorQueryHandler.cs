using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Application.Contracts.Features.Authentication.Queries;
using MediatR;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class GetAllDoctorQueryHandler : IRequestHandler<GetAllDoctorsQuery, Result<IEnumerable<DoctorDto>>>
    {
        private readonly IDoctorRepository doctorRepository;
        public GetAllDoctorQueryHandler(IDoctorRepository _doctorRepository)
        {
            doctorRepository = _doctorRepository;
        }
        public async Task<Result<IEnumerable<DoctorDto>>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            var users = await doctorRepository.GetAllDoctors(request.pageSize, request.pageNumber);
            return Result<IEnumerable<DoctorDto>>.Success(users);
        }
    }
}
