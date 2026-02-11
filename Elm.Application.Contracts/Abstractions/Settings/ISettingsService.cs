namespace Elm.Application.Contracts.Abstractions.Settings
{
    public interface ISettingsService
    {
        Task<Result<bool>> AddSettingsAsync(string key, string value);
        Task<Result<bool>> UpdateSettingsAsync(string key, string value);
        Task<Result<List<SettingsDto>>> GetAllSettingsAsync();
        Task<int> GetMaxQuestionsAsync();
        Task<long> GetMaxStorageBytesAsync();
        void ClearCache();
    }
}
