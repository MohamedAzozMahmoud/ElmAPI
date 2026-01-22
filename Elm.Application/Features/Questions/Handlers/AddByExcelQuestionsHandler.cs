using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Excel;
using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Questions.Handlers
{
    public sealed class AddByExcelQuestionsHandler : IRequestHandler<AddByExcelQuestionsCommand, Result<bool>>
    {
        private readonly IQuestionRepository repository;
        private readonly IExcelReader excelService;
        public AddByExcelQuestionsHandler(IQuestionRepository _repository, IExcelReader _excelService)
        {
            repository = _repository;
            excelService = _excelService;
        }
        public async Task<Result<bool>> Handle(AddByExcelQuestionsCommand request, CancellationToken cancellationToken)
        {
            var addquestions = excelService.ReadExcelFile<TemplateQuestionsDto>(request.ExcelFile);
            if (addquestions.Any(q => string.IsNullOrWhiteSpace(q.Content) || string.IsNullOrWhiteSpace(q.QuestionType)))
            {
                return Result<bool>.Failure("Invalid question data found.");
            }
            var result = await repository.AddRingQuestionsFromExcel(request.questionBankId, addquestions.ToList());
            return Result<bool>.Success(result.Data);
        }
    }
}
