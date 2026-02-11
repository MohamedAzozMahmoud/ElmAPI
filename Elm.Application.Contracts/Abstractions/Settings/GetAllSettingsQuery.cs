using MediatR;

namespace Elm.Application.Contracts.Abstractions.Settings
{
    public record GetAllSettingsQuery : IRequest<Result<List<SettingsDto>>>;
}
