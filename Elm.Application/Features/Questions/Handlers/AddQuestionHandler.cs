using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using Elm.Domain.Enums;
using MediatR;

namespace Elm.Application.Features.Questions.Handlers
{
    public sealed class AddQuestionHandler : IRequestHandler<AddQuestionCommand, Result<QuestionsDto>>
    {
        private readonly IQuestionRepository repository;
        private readonly IMapper mapper;
        public AddQuestionHandler(IQuestionRepository _repository, IMapper mapper)
        {
            repository = _repository;
            this.mapper = mapper;
        }
        public async Task<Result<QuestionsDto>> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = new Question
            {
                QuestionBankId = request.questionBankId,
                Content = request.QuestionsDto.Content,
                Options = request.QuestionsDto.Options.Select(o => new Option
                {
                    Content = o.Content,
                    IsCorrect = o.IsCorrect
                }).ToList(),
                QuestionType = Enum.Parse<QuestionType>(request.QuestionsDto.QuestionType),
            };
            var question1 = await repository.AddAsync(question);
            var questionDto = mapper.Map<QuestionsDto>(question1);
            return Result<QuestionsDto>.Success(questionDto);
        }
    }
}
