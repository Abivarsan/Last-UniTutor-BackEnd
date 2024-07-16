using AutoMapper;
using UniTutor.DTO;
using UniTutor.Model;

namespace UniTutor.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tutor, TutorRegistration>().ReverseMap();
            CreateMap<Tutor, UpdateTutorDto>().ReverseMap();

            CreateMap<Student, StudentRegistration>().ReverseMap();
            CreateMap<Student, UpdateStudentDto>().ReverseMap();

            CreateMap<Subject, SubjectRequestDto>().ReverseMap();


            CreateMap<Subject, SubjectRequestDto>().ReverseMap();


        }

    }
}