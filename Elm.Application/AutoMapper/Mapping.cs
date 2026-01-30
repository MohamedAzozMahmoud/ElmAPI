using AutoMapper;
using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Features.Options.DTOs;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Features.Roles.DTOs;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Features.University.DTOs;
using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Domain.Entities;

namespace Elm.Application.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {

            #region Permission Mappings

            CreateMap<Elm.Domain.Entities.Permissions, PermissionDto>()
                .ReverseMap();

            #endregion

            #region University Mappings

            CreateMap<University, UniversityDto>()
                .ReverseMap();
            CreateMap<University, UniversityDetialsDto>()
                .ReverseMap();

            #endregion

            #region College Mappings

            CreateMap<College, CollegeDto>()
                .ReverseMap();
            CreateMap<College, GetCollegeDto>()
                .ReverseMap();

            #endregion

            #region Year Mappings

            CreateMap<Year, GetYearDto>()
                .ReverseMap();
            CreateMap<Year, YearDto>()
                .ReverseMap();

            #endregion

            #region Department Mappings

            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.Type,
                           opt => opt.MapFrom(src => src.Type.ToString()))
                .ReverseMap();
            CreateMap<Department, GetDepartmentDto>()
                .ForMember(dest => dest.Type,
                           opt => opt.MapFrom(src => src.Type.ToString()))
                .ReverseMap();

            #endregion

            #region Subject Mappings

            CreateMap<Subject, SubjectDto>()
                .ReverseMap();
            CreateMap<Subject, GetSubjectDto>()
                .ReverseMap();

            #endregion

            #region Curriculum Mappings

            CreateMap<Curriculum, CurriculumDto>()
                .ReverseMap();

            #endregion

            #region QuestionsBank Mappings

            CreateMap<QuestionsBank, QuestionsBankDto>()
                .ReverseMap();

            #endregion

            #region Question Mappings

            CreateMap<Question, QuestionsDto>()
               .ForMember(dest => dest.QuestionType,
                          opt => opt.MapFrom(src => src.QuestionType.ToString()))
               .ReverseMap();

            CreateMap<Question, AddQuestionsDto>()
                .ForMember(dest => dest.QuestionType,
                           opt => opt.MapFrom(src => src.QuestionType.ToString()))
                .ReverseMap();


            #endregion

            #region Option Mappings

            CreateMap<Option, OptionsDto>()
                .ReverseMap();

            CreateMap<Option, AddOptionsDto>()
                .ReverseMap();

            #endregion

            #region Role Mappings

            CreateMap<RoleDto, string>()
                .ConvertUsing(src => src.Name);
            #endregion

        }
    }
}
