using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.Questions.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Questions.Handlers
{
    public sealed class GetQuestionsByIdHandler : IRequestHandler<GetQuestionByIdQuery, Result<QuestionsDto>>
    {
        private readonly IQuestionRepository repository;
        public GetQuestionsByIdHandler(IQuestionRepository _repository)
        {
            repository = _repository;
        }
        public async Task<Result<QuestionsDto>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var question = await repository.GetQuestionById(request.id);
            if (!question.IsSuccess)
            {
                return Result<QuestionsDto>.Failure(question.Message, question.StatusCode);
            }
            return question;
        }
    }
}
