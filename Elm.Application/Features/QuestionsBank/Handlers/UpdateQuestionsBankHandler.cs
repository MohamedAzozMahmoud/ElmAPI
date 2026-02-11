using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class UpdateQuestionsBankHandler : IRequestHandler<UpdateQuestionsBankCommand, Result<bool>>
    {
        private readonly IGenericRepository<Elm.Domain.Entities.QuestionsBank> repository;
        public UpdateQuestionsBankHandler(IGenericRepository<Elm.Domain.Entities.QuestionsBank> repository)
        {
            this.repository = repository;
        }
        public async Task<Result<bool>> Handle(UpdateQuestionsBankCommand request, CancellationToken cancellationToken)
        {
            var questionsBank = await repository.GetByIdAsync(request.id);
            if (questionsBank == null)
            {
                return Result<bool>.Failure("Questions Bank not found", 404);
            }
            questionsBank.Name = request.name;
            questionsBank.CurriculumId = request.curriculumId;
            var result = await repository.UpdateAsync(questionsBank);
            if (!result)
            {
                return Result<bool>.Failure("Failed to update Questions Bank");
            }
            return Result<bool>.Success(result);
        }
    }
}
