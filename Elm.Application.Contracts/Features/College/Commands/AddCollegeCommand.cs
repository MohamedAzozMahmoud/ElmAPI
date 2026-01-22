using Elm.Application.Contracts.Features.College.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.College.Commands
{
    public record AddCollegeCommand
      (string name, int UniversityId) : IRequest<Result<CollegeDto>>;
}
