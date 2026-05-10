using Core.Entities;
using Mapster;

namespace StudentApi.Features.StudentFeatures
{
    public class StudentMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Student, StudentRepresentationModel>()
                .Map(dest => dest.id,src => src.id)
                .Map(dest => dest.age,src => src.age)
                .Map(dest => dest.course,src => src.course)
                .Map(dest => dest.status,src => src.status)
                .Map(dest => dest.email,src => src.email)
                .Map(dest => dest.name,src => src.name);
            config.NewConfig<StudentRepresentationModel, Student>()
                .Map(dest => dest.id, src => src.id)
                .Map(dest => dest.age, src => src.age)
                .Map(dest => dest.course, src => src.course)
                .Map(dest => dest.status, src => src.status)
                .Map(dest => dest.email, src => src.email)
                .Map(dest => dest.name, src => src.name);
            config.NewConfig<StudentInputRepresentationModel, Student>()
                .Map(dest => dest.age, src => src.age)
                .Map(dest => dest.course, src => src.course)
                .Map(dest => dest.status, src => src.status)
                .Map(dest => dest.email, src => src.email)
                .Map(dest => dest.name, src => src.name);
        }
    }
}
