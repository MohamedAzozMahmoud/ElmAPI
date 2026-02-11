using MediatR;

namespace Elm.Application.Contracts.Abstractions.Settings
{
    public record AddSettingsCommand(
        string Key,
        string Value
    ) : IRequest<Result<bool>>;
}
