using Cortex.Mediator.Commands;

namespace StudentApi.Features.StudentFeatures.UpdateStudent
{
    public class UpdateStudentRequestModel : ICommand<UpdateStudentResponseModel>
    {
        public Guid id { get; set; }
        public StudentInputRepresentationModel student { get; set; }
    }
}
