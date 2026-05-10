using Core.Entities;
using Core.Enum;
using Infastructure.Repository.Base;
using MapsterMapper;
using NSubstitute;
using Shouldly;
using StudentApi.Features.StudentFeatures;
using StudentApi.Features.StudentFeatures.UpdateStudent;
using Xunit;

namespace StudentApiTest.Features.StudentFeatures
{
    public class UpdateStudentHandlerTests
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UpdateStudentHandler _handler;

        public UpdateStudentHandlerTests()
        {
            _studentRepository = Substitute.For<IRepository<Student>>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new UpdateStudentHandler(_studentRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_StudentNotFound_ShouldReturn404()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var request = new UpdateStudentRequestModel { id = studentId };
            _studentRepository.GetByIdAsync(studentId).Returns((Student)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.status.ShouldBe(404);
            result.message.ShouldBe("Student not found.");
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldUpdateAndReturnSuccess()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var existingStudent = new Student { id = studentId, name = "Old Name" };
            var request = new UpdateStudentRequestModel
            {
                id = studentId,
                student = new StudentInputRepresentationModel
                {
                    name = "New Name",
                    email = "new@example.com",
                    age = 21,
                    course = "Math",
                    status = EntityStatus.Active
                }
            };

            _studentRepository.GetByIdAsync(studentId).Returns(existingStudent);
            
            var updatedModel = new StudentRepresentationModel { name = "New Name" };
            _mapper.Map<StudentRepresentationModel>(existingStudent).Returns(updatedModel);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.status.ShouldBe(200);
            result.message.ShouldBe("Student updated successfully.");
            existingStudent.name.ShouldBe("New Name");

            await _studentRepository.Received(1).UpdateAsync(existingStudent);
            await _unitOfWork.Received(1).CommitAsync();
        }
    }
}
