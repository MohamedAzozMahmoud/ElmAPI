using Elm.Application.Contracts.Features.Year.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Year.Commands
{
    public record AddYearCommand(string name, int collegeId) : IRequest<Result<YearDto>>;
}
