using Core.Entities;
using Core.Enum;
using Infastructure.Repository.Base;
using MapsterMapper;
using NSubstitute;
using Shouldly;
using StudentApi.Features.StudentFeatures;
using StudentApi.Features.StudentFeatures.AddStudent;
using Xunit;

namespace StudentApiTest.Features.StudentFeatures
{
    public class AddStudentHandlerTests
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AddStudentHandler _handler;

        public AddStudentHandlerTests()
        {
            _studentRepository = Substitute.For<IRepository<Student>>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new AddStudentHandler(_studentRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldAddStudentAndReturnSuccess()
        {
            // Arrange
            var request = new AddStudentRequestModel
            {
                student = new StudentInputRepresentationModel
                {
                    name = "Test Student",
                    email = "test@example.com",
                    age = 20,
                    course = "Computer Science",
                    status = EntityStatus.Active
                }
            };

            var studentEntity = new Student { name = request.student.name };
            var responseStudentModel = new StudentRepresentationModel { name = request.student.name };

            _mapper.Map<Student>(request.student).Returns(studentEntity);
            _mapper.Map<StudentRepresentationModel>(studentEntity).Returns(responseStudentModel);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.status.ShouldBe(200);
            result.message.ShouldBe("Student added successfully.");
            result.student.name.ShouldBe(request.student.name);

            await _studentRepository.Received(1).AddAsync(Arg.Is<Student>(s => s.name == request.student.name));
            await _unitOfWork.Received(1).CommitAsync();
        }
    }
}
