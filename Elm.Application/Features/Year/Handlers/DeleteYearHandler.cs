using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Year.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Year.Handlers
{
    public sealed class DeleteYearHandler : IRequestHandler<DeleteYearCommand, Result<bool>>
    {
        private readonly IYearRepository yearRepository;
        public DeleteYearHandler(IYearRepository yearRepository)
        {
            this.yearRepository = yearRepository;
        }
        public async Task<Result<bool>> Handle(DeleteYearCommand request, CancellationToken cancellationToken)
        {
            var result = await yearRepository.DeleteAsync(request.Id);
            if (result)
            {
                return Result<bool>.Success(true);
            }
            return Result<bool>.Failure("Not deleted");
        }
    }
}
