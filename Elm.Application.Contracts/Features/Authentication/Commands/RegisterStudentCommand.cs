using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Commands
{
    public record RegisterStudentCommand(string UserName, string Password, string ConfirmPassword, string FullName, int DepartmentId, int YearId) : IRequest<Result<bool>>;
}
