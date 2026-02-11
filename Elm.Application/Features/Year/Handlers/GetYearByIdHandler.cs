using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Application.Contracts.Features.Year.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Year.Handlers
{
    public sealed class GetYearByIdHandler : IRequestHandler<GetYearByIdQuery, Result<GetYearDto>>
    {
        private readonly IYearRepository yearRepository;
        public GetYearByIdHandler(IYearRepository yearRepository)
        {
            this.yearRepository = yearRepository;
        }
        public async Task<Result<GetYearDto>> Handle(GetYearByIdQuery request, CancellationToken cancellationToken)
        {
            var year = await yearRepository.GetYearByIdAsync(request.Id);
            if (year == null)
            {
                return Result<GetYearDto>.Failure("Year not found");
            }
            return Result<GetYearDto>.Success(year);
        }
    }
}
