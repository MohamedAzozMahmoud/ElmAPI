using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Questions.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Questions.Handlers
{
    public sealed class GetBankCountQueryHandler : IRequestHandler<GetBankCountQuery, Result<int>>
    {
        private readonly IQuestionRepository repository;
        public GetBankCountQueryHandler(IQuestionRepository _repository)
        {
            repository = _repository;
        }
        public async Task<Result<int>> Handle(GetBankCountQuery request, CancellationToken cancellationToken)
        {
            var count = await repository.GetBankInfoAsync(request.bankId);
            if (count == null)
            {
                return Result<int>.Failure("Question bank not found.", 404);
            }
            return Result<int>.Success(count.TotalQuestions);
        }
    }
}
