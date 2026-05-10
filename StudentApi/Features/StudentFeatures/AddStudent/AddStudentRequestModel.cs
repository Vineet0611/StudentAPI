using Core.Enum;
using Cortex.Mediator.Commands;
using System.Windows.Input;

namespace StudentApi.Features.StudentFeatures.AddStudent
{
    public class AddStudentRequestModel : ICommand<AddStudentResponseModel>
    {
        public StudentInputRepresentationModel student { get; set; }
    }
}
