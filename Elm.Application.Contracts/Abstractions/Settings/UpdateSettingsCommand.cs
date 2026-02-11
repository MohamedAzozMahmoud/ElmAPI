using MediatR;

namespace Elm.Application.Contracts.Abstractions.Settings
{
    public record UpdateSettingsCommand(
        string Key,
        string Value
    ) : IRequest<Result<bool>>;
}
