using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class PublishedCurriculumCommandHandler : IRequestHandler<PublishedCurriculumCommand, Result<bool>>
    {
        private readonly ICurriculumRepository repository;
        public PublishedCurriculumCommandHandler(ICurriculumRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<bool>> Handle(PublishedCurriculumCommand request, CancellationToken cancellationToken)
        {
            var existingCurriculum = await repository.GetByIdAsync(request.Id);
            if (existingCurriculum == null)
            {
                return Result<bool>.Failure("لا يوجد منهج دراسي", 404);
            }
            existingCurriculum.IsPublished = !existingCurriculum.IsPublished; // Toggle the published state
            await repository.UpdateAsync(existingCurriculum);
            return Result<bool>.Success(true);
        }
    }
}
