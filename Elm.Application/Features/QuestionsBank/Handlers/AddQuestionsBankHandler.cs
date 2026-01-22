using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class AddQuestionsBankHandler : IRequestHandler<AddQuestionsBankCommand, Result<QuestionsBankDto>>
    {
        private readonly IGenericRepository<Elm.Domain.Entities.QuestionsBank> repository;
        private readonly IMapper mapper;

        public AddQuestionsBankHandler(IGenericRepository<Elm.Domain.Entities.QuestionsBank> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<QuestionsBankDto>> Handle(AddQuestionsBankCommand request, CancellationToken cancellationToken)
        {
            var questionsBank = new Elm.Domain.Entities.QuestionsBank { Name = request.name, CurriculumId = request.curriculumId };
            var addedQuestionsBank = await repository.AddAsync(questionsBank);
            var questionsBankDto = mapper.Map<QuestionsBankDto>(addedQuestionsBank);
            return Result<QuestionsBankDto>.Success(questionsBankDto);
        }
    }
}
