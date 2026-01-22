using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class UpdateQuestionsBankHandler : IRequestHandler<UpdateQuestionsBankCommand, Result<QuestionsBankDto>>
    {
        private readonly IGenericRepository<Elm.Domain.Entities.QuestionsBank> repository;
        private readonly IMapper mapper;
        public UpdateQuestionsBankHandler(IGenericRepository<Elm.Domain.Entities.QuestionsBank> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<QuestionsBankDto>> Handle(UpdateQuestionsBankCommand request, CancellationToken cancellationToken)
        {
            var questionsBank = await repository.GetByIdAsync(request.id);
            if (questionsBank == null)
            {
                return Result<QuestionsBankDto>.Failure("Questions Bank not found", 404);
            }
            questionsBank.Name = request.name;
            questionsBank.CurriculumId = request.curriculumId;
            var updatedQuestionsBank = await repository.UpdateAsync(questionsBank);
            var questionsBankDto = mapper.Map<QuestionsBankDto>(updatedQuestionsBank);
            return Result<QuestionsBankDto>.Success(questionsBankDto);
        }
    }
}
