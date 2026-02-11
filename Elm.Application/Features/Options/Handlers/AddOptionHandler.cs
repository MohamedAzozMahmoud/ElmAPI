using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Options.Commands;
using Elm.Application.Contracts.Features.Options.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Mapper.Elm.Application.Mappers;
using Elm.Domain.Entities;
using MediatR;

namespace Elm.Application.Features.Options.Handlers
{
    public sealed class AddOptionHandler : IRequestHandler<AddOptionCommand, Result<OptionsDto>>
    {
        private readonly IGenericRepository<Option> repository;
        private readonly MappingProvider mapping;
        public AddOptionHandler(IGenericRepository<Option> repository, MappingProvider mapping)
        {
            this.repository = repository;
            this.mapping = mapping;
        }
        public async Task<Result<OptionsDto>> Handle(AddOptionCommand request, CancellationToken cancellationToken)
        {
            var option = new Option
            {
                Content = request.content,
                IsCorrect = request.isCorrect,
                QuestionId = request.questionId
            };
            var addedOption = await repository.AddAsync(option);
            if (addedOption == null)
            {
                return Result<OptionsDto>.Failure("Failed to add option");
            }
            return Result<OptionsDto>.Success(mapping.MapToDto(addedOption));
        }
    }
}
