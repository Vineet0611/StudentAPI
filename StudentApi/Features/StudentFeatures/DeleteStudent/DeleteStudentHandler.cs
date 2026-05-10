using Core.Entities;
using Core.Enum;
using Cortex.Mediator.Commands;
using Infastructure.Repository.Base;

namespace StudentApi.Features.StudentFeatures.DeleteStudent
{
    public class DeleteStudentHandler(
        IRepository<Student> studentRepository,
        IUnitOfWork _unitOfWork
        ) : ICommandHandler<DeleteStudentRequestModel, DeleteStudentResponseModel>
    {
        public async Task<DeleteStudentResponseModel> Handle(DeleteStudentRequestModel request, CancellationToken cancellationToken)
        {
            var response = new DeleteStudentResponseModel();
            
            var existingStudent = await studentRepository.GetByIdAsync(request.id);
            if (existingStudent == null)
            {
                response.status = 404;
                response.message = "Student not found.";
                return response;
            }

            existingStudent.status = EntityStatus.Inactive;
            await studentRepository.UpdateAsync(existingStudent);
            await _unitOfWork.CommitAsync();

            response.status = 200;
            response.message = "Student deleted successfully.";
            
            return response;
        }
    }
}
