using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Options.Commands;
using Elm.Application.Contracts.Features.Options.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using MediatR;

namespace Elm.Application.Features.Options.Handlers
{
    public sealed class AddOptionHandler : IRequestHandler<AddOptionCommand, Result<OptionsDto>>
    {
        private readonly IGenericRepository<Option> repository;
        private readonly IMapper mapper;
        public AddOptionHandler(IGenericRepository<Option> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<OptionsDto>> Handle(AddOptionCommand request, CancellationToken cancellationToken)
        {
            var option = new Option
            {
                Content = request.content,
                IsCorrect = request.isCorrect
                ,
                QuestionId = request.questionId
            };
            var addedOption = await repository.AddAsync(option);
            if (addedOption == null)
            {
                return Result<OptionsDto>.Failure("Failed to add option");
            }
            return Result<OptionsDto>.Success(mapper.Map<OptionsDto>(addedOption));
        }
    }
}
