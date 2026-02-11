using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Features.Options.DTOs;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Features.University.DTOs;
using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Domain.Entities;
using Riok.Mapperly.Abstractions;
namespace Elm.Application.Mapper
{

    namespace Elm.Application.Mappers
    {
        [Mapper]
        public partial class MappingProvider
        {
            #region User Mappings

            [MapProperty(nameof(AppUser.Id), nameof(UserDto.UserId))]
            public partial UserDto MapToDto(AppUser user);

            public partial List<UserDto> MapToDtoList(List<AppUser> users);
            #endregion

            #region Permission Mappings
            public partial PermissionDto MapToDto(Permissions permission);
            public partial List<PermissionDto> MapToDtoList(List<Permissions> permissions);
            #endregion

            #region University Mappings
            public partial UniversityDto MapToDto(University university);

            #endregion

            #region College Mappings
            public partial CollegeDto MapToDto(College college);
            #endregion

            #region Year Mappings

            public partial YearDto MapToDto(Year year);

            #endregion

            #region Department Mappings
            public partial GetDepartmentDto MapToGetDto(Department department);
            public partial DepartmentDto MapToDto(Department department);

            #endregion

            #region Subject Mappings
            public partial SubjectDto MapToDto(Subject subject);
            public partial GetSubjectDto MapToGetDto(Subject subject);
            #endregion

            #region Question Bank Mappings

            public partial QuestionsBankDto MapToDto(QuestionsBank questionBank);

            #endregion

            #region Question Mappings
            public partial QuestionsDto MapToDto(Question question);
            #endregion

            #region Option Mappings
            public partial OptionsDto MapToDto(Option option);
            #endregion

            #region Curriculum Mappings
            public partial CurriculumDto MapToDto(Curriculum curriculum);

            #endregion


        }
    }
}
