using Elm.Application.Contracts.Features.Authentication.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Queries
{
    public record GetAllDoctorsQuery(int pageSize, int pageNumber) : IRequest<Result<IEnumerable<DoctorDto>>>;
}
