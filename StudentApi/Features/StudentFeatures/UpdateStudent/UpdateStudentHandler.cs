using Core.Entities;
using Cortex.Mediator.Commands;
using Infastructure.Repository.Base;
using MapsterMapper;

namespace StudentApi.Features.StudentFeatures.UpdateStudent
{
    public class UpdateStudentHandler(
        IRepository<Student> studentRepository,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : ICommandHandler<UpdateStudentRequestModel, UpdateStudentResponseModel>
    {
        public async Task<UpdateStudentResponseModel> Handle(UpdateStudentRequestModel request, CancellationToken cancellationToken)
        {
            var response = new UpdateStudentResponseModel();
            
            var existingStudent = await studentRepository.GetByIdAsync(request.id);
            if (existingStudent == null)
            {
                response.status = 404;
                response.message = "Student not found.";
                return response;
            }

            // Update properties
            existingStudent.name = request.student.name;
            existingStudent.email = request.student.email;
            existingStudent.age = request.student.age;
            existingStudent.course = request.student.course;
            existingStudent.status = request.student.status;

            await studentRepository.UpdateAsync(existingStudent);
            await _unitOfWork.CommitAsync();

            response.student = _mapper.Map<StudentRepresentationModel>(existingStudent);
            response.status = 200;
            response.message = "Student updated successfully.";
            
            return response;
        }
    }
}
