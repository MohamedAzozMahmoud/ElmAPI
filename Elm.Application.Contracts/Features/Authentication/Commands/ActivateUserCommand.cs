using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Commands
{
    // I want you to made a record Use For Transfer Satut User properity IsActived from false to true:
    public record ActivateUserCommand(string UserId) : IRequest<Result<bool>>;
}
