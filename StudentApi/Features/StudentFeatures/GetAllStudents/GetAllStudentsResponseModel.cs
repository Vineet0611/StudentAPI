using Infastructure.Header;

namespace StudentApi.Features.StudentFeatures.GetAllStudents
{
    public class GetAllStudentsResponseModel : BaseResponseModel
    {
        public List<StudentRepresentationModel> Students { get; set; } = new List<StudentRepresentationModel>();
    }
}
