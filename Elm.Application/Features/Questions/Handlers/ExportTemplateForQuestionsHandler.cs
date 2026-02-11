using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Excel;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.Questions.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Questions.Handlers
{
    public sealed class ExportTemplateForQuestionsHandler : IRequestHandler<ExportTemplateForQuestionsQuery, Result<MemoryStream>>
    {
        private readonly IQuestionBankRepository repository;
        private readonly IExcelWriter writer;
        public ExportTemplateForQuestionsHandler(IQuestionBankRepository _repository, IExcelWriter _excelService)
        {
            repository = _repository;
            writer = _excelService;
        }
        public async Task<Result<MemoryStream>> Handle(ExportTemplateForQuestionsQuery request, CancellationToken cancellationToken)
        {
            var questions = await repository.FindAsync(x => x.Id == request.QuestionBankId);
            if (questions is null)
            {
                return Result<MemoryStream>.Failure("Question bank not found.");
            }
            else
            {
                var template = writer.WriteExcelFile(new List<TemplateQuestionsDto>(), questions.Name);
                if (template == null || template.Length == 0)
                {
                    return Result<MemoryStream>.Failure("Failed to generate Excel template.");
                }
                if (template.CanSeek) template.Position = 0;
                return Result<MemoryStream>.Success(template);
            }
        }
    }
}
