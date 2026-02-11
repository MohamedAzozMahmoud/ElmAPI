using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Settings;
using MediatR;

namespace Elm.Application.SettingsHandler
{
    public sealed class AddSettingsCommandHandler : IRequestHandler<AddSettingsCommand, Result<bool>>
    {
        private readonly ISettingsService _settingsService;
        public AddSettingsCommandHandler(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        public async Task<Result<bool>> Handle(AddSettingsCommand request, CancellationToken cancellationToken)
        {
            var result = await _settingsService.AddSettingsAsync(request.Key, request.Value);
            if (result.IsSuccess)
            {
                return Result<bool>.Success(true, "Settings added successfully.");
            }
            else
            {
                return Result<bool>.Failure(result.Message ?? "Failed to add settings.");
            }
        }
    }
}
