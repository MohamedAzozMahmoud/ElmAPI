using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Features.Curriculum.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class GetCurriculumByDoctorIdHandler : IRequestHandler<GetCurriculumByDoctorIdQuery, Result<List<GetCurriculumDto>>>
    {
        private readonly ICurriculumRepository repository;
        private readonly IDoctorRepository doctorRepsitory;
        public GetCurriculumByDoctorIdHandler(ICurriculumRepository repository, IDoctorRepository doctorRepsitory)
        {
            this.repository = repository;
            this.doctorRepsitory = doctorRepsitory;
        }
        public async Task<Result<List<GetCurriculumDto>>> Handle(GetCurriculumByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            var doctor = await doctorRepsitory.GetDoctor(request.UserId);
            if (doctor == null)
            {
                return Result<List<GetCurriculumDto>>.Failure("Doctor not found");
            }
            var curriculums = await repository.GetByDoctorIdAsync(doctor.Id);
            if (curriculums == null)
            {
                return Result<List<GetCurriculumDto>>.Failure("Curriculum not found");
            }
            return Result<List<GetCurriculumDto>>.Success(curriculums);
        }
    }
}
