using Infastructure.Header;

namespace StudentApi.Features.StudentFeatures.UpdateStudent
{
    public class UpdateStudentResponseModel : BaseResponseModel
    {
        public StudentRepresentationModel student { get; set; }
    }
}
