using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Options.Commands;
using Elm.Application.Contracts.Features.Options.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using MediatR;

namespace Elm.Application.Features.Options.Handlers
{
    public sealed class UpdateOptionHandler : IRequestHandler<UpdateOptionCommand, Result<OptionsDto>>
    {
        private readonly IGenericRepository<Option> repository;
        private readonly IMapper mapper;
        public UpdateOptionHandler(IGenericRepository<Option> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<OptionsDto>> Handle(UpdateOptionCommand request, CancellationToken cancellationToken)
        {
            var option = await repository.GetByIdAsync(request.optionId);
            if (option == null)
            {
                return Result<OptionsDto>.Failure("Option not found", 404);
            }
            option.Content = request.content;
            option.IsCorrect = request.isCorrect;
            var updatedOption = await repository.UpdateAsync(option);
            return Result<OptionsDto>.Success(mapper.Map<OptionsDto>(updatedOption));
        }
    }
}
