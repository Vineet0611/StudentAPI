using Cortex.Mediator.Commands;

namespace StudentApi.Features.StudentFeatures.DeleteStudent
{
    public class DeleteStudentRequestModel : ICommand<DeleteStudentResponseModel>
    {
        public Guid id { get; set; }
    }
}
