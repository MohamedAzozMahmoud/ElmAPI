using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Commands
{
    public record RegisterDoctorCommand(string UserName, string Password, string ConfirmPassword, string FullName, string Title) : IRequest<Result<bool>>;
}
