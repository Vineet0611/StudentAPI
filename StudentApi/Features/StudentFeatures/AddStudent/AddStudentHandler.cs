using Core.Entities;
using Core.Enum;
using Cortex.Mediator.Commands;
using Infastructure.Repository.Base;
using MapsterMapper;

namespace StudentApi.Features.StudentFeatures.AddStudent
{
    public class AddStudentHandler(
        IRepository<Student> studentRepository,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : ICommandHandler<AddStudentRequestModel, AddStudentResponseModel>
    {
        public async Task<AddStudentResponseModel> Handle(AddStudentRequestModel request, CancellationToken cancellationToken)
        {
            return await AddNewStudent(request, cancellationToken);
        }

        private async Task<AddStudentResponseModel> AddNewStudent(AddStudentRequestModel request, CancellationToken cancellationToken)
         {
            var response = new AddStudentResponseModel();
            var newStudent = _mapper.Map<Student>(request.student);
            newStudent.id = Guid.NewGuid();
            newStudent.created_date = DateTime.UtcNow;
            newStudent.status = EntityStatus.Active;
            await studentRepository.AddAsync(newStudent);
            await _unitOfWork.CommitAsync();
            response.student = _mapper.Map<StudentRepresentationModel>(newStudent);
            response.status= 200;
            response.message = "Student added successfully.";    
            return response;
        }
    }
}
