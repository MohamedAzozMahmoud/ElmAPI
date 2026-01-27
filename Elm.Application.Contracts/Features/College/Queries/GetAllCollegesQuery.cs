using Elm.Application.Contracts.Features.College.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.College.Queries
{
    public record GetAllCollegesQuery(int universityId) : IRequest<Result<List<GetCollegeDto>>>;
}
