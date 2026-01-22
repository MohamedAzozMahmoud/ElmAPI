using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Enums;
using MediatR;

namespace Elm.Application.Features.Questions.Handlers
{
    public sealed class UpdateQuestionHandler : IRequestHandler<UpdateQuestionCommand, Result<QuestionsDto>>
    {
        private readonly IQuestionRepository repository;
        private readonly IMapper mapper;
        public UpdateQuestionHandler(IQuestionRepository _repository, IMapper mapper)
        {
            repository = _repository;
            this.mapper = mapper;
        }
        public async Task<Result<QuestionsDto>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var QuestionResult = await repository.GetByIdAsync(request.Id);
            if (QuestionResult is null)
            {
                return Result<QuestionsDto>.Failure("Question not found", 404);
            }
            QuestionResult.Content = request.Content;
            QuestionResult.QuestionType = Enum.Parse<QuestionType>(request.QuestionType);

            var updatedQuestion = await repository.UpdateAsync(QuestionResult);
            var questionDto = mapper.Map<QuestionsDto>(updatedQuestion);
            return Result<QuestionsDto>.Success(questionDto);
        }
    }
}
