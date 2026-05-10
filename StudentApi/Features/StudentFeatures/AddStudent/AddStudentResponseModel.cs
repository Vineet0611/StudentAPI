using Infastructure.Header;
using System.Windows.Input;

namespace StudentApi.Features.StudentFeatures.AddStudent
{
    public class AddStudentResponseModel  : BaseResponseModel
    {
        public StudentRepresentationModel student { get; set; }
    }
}
