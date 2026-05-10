using Core.Enum;

namespace StudentApi.Features.StudentFeatures
{
    public class StudentInputRepresentationModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public int age { get; set; }
        public string course { get; set; }
        public EntityStatus status { get; set; }
    }
}
