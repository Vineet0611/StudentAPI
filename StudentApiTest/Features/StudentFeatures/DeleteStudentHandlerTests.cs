using Core.Entities;
using Core.Enum;
using Infastructure.Repository.Base;
using NSubstitute;
using Shouldly;
using StudentApi.Features.StudentFeatures.DeleteStudent;
using Xunit;

namespace StudentApiTest.Features.StudentFeatures
{
    public class DeleteStudentHandlerTests
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DeleteStudentHandler _handler;

        public DeleteStudentHandlerTests()
        {
            _studentRepository = Substitute.For<IRepository<Student>>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new DeleteStudentHandler(_studentRepository, _unitOfWork);
        }

        [Fact]
        public async Task Handle_StudentNotFound_ShouldReturn404()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var request = new DeleteStudentRequestModel { id = studentId };
            _studentRepository.GetByIdAsync(studentId).Returns((Student)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.status.ShouldBe(404);
            result.message.ShouldBe("Student not found.");
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldDeactivateAndReturnSuccess()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var existingStudent = new Student { id = studentId, status = EntityStatus.Active };
            var request = new DeleteStudentRequestModel { id = studentId };

            _studentRepository.GetByIdAsync(studentId).Returns(existingStudent);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.status.ShouldBe(200);
            result.message.ShouldBe("Student deleted successfully.");
            existingStudent.status.ShouldBe(EntityStatus.Inactive);

            await _studentRepository.Received(1).UpdateAsync(existingStudent);
            await _unitOfWork.Received(1).CommitAsync();
        }
    }
}
