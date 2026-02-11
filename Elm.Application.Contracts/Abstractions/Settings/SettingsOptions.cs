namespace Elm.Application.Contracts.Abstractions.Settings
{
    public record SettingsOptions(int MaxQuestions = 300,
         long MaxStorageBytes = 10485760
);
}
