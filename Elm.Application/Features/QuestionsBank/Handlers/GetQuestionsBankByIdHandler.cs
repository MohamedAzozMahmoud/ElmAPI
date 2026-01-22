using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Features.QuestionsBank.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class GetQuestionsBankByIdHandler : IRequestHandler<GetQuestionsBankByIdQuery, Result<QuestionsBankDto>>
    {
        private readonly IGenericRepository<Elm.Domain.Entities.QuestionsBank> repository;
        private readonly IMapper mapper;
        public GetQuestionsBankByIdHandler(IGenericRepository<Elm.Domain.Entities.QuestionsBank> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<QuestionsBankDto>> Handle(GetQuestionsBankByIdQuery request, CancellationToken cancellationToken)
        {
            var questionsBank = await repository.GetByIdAsync(request.id);
            if (questionsBank == null)
            {
                return Result<QuestionsBankDto>.Failure("Questions Bank not found", 404);
            }
            var questionsBankDto = mapper.Map<QuestionsBankDto>(questionsBank);
            return Result<QuestionsBankDto>.Success(questionsBankDto);
        }
    }
}
