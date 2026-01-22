namespace Elm.Application.Contracts.Features.Test.DTOs
{
    public record QuestionWithOptions(
        int Id,
        string Content,
        string QuestionType,
        List<OptionData> Options
    );
}
