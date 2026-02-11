using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Mapper.Elm.Application.Mappers;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class AddQuestionsBankHandler : IRequestHandler<AddQuestionsBankCommand, Result<QuestionsBankDto>>
    {
        private readonly IGenericRepository<Elm.Domain.Entities.QuestionsBank> repository;
        private readonly MappingProvider mapping;

        public AddQuestionsBankHandler(IGenericRepository<Elm.Domain.Entities.QuestionsBank> repository
            , MappingProvider mapping)
        {
            this.repository = repository;
            this.mapping = mapping;
        }

        public async Task<Result<QuestionsBankDto>> Handle(AddQuestionsBankCommand request, CancellationToken cancellationToken)
        {
            var questionsBank = new Elm.Domain.Entities.QuestionsBank { Name = request.name, CurriculumId = request.curriculumId };
            var addedQuestionsBank = await repository.AddAsync(questionsBank);
            if (addedQuestionsBank is null)
            {
                return Result<QuestionsBankDto>.Failure("Failed to add questions bank");
            }
            var questionsBankDto = mapping.MapToDto(addedQuestionsBank);
            return Result<QuestionsBankDto>.Success(questionsBankDto);
        }
    }
}
