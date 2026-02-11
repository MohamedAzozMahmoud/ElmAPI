using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Settings;
using MediatR;

namespace Elm.Application.SettingsHandler
{
    public sealed class UpdateSettingsCommandHandler : IRequestHandler<UpdateSettingsCommand, Result<bool>>
    {
        private readonly ISettingsService _settingsService;
        public UpdateSettingsCommandHandler(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        public async Task<Result<bool>> Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
        {
            var result = await _settingsService.UpdateSettingsAsync(request.Key, request.Value);
            if (result.IsSuccess)
            {
                return Result<bool>.Success(true, "Settings updated successfully.");
            }
            else
            {
                return Result<bool>.Failure(result.Message ?? "Failed to update settings.");
            }
        }
    }
}
