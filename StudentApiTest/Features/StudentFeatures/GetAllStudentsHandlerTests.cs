using Core.Entities;
using Core.Enum;
using Infastructure.Repository.Base;
using MapsterMapper;
using MockQueryable.NSubstitute;
using NSubstitute;
using Shouldly;
using StudentApi.Features.StudentFeatures;
using StudentApi.Features.StudentFeatures.GetAllStudents;
using Xunit;

namespace StudentApiTest.Features.StudentFeatures
{
    public class GetAllStudentsHandlerTests
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly GetAllStudentsHandler _handler;

        public GetAllStudentsHandlerTests()
        {
            _studentRepository = Substitute.For<IRepository<Student>>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetAllStudentsHandler(_studentRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnActiveStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { id = Guid.NewGuid(), name = "Active Student", status = EntityStatus.Active },
                new Student { id = Guid.NewGuid(), name = "Inactive Student", status = EntityStatus.Inactive }
            };

            var mockQuery = students.AsQueryable().BuildMock();
            _studentRepository.GetAllQuery().Returns(mockQuery);

            var expectedResponseModels = new List<StudentRepresentationModel>
            {
                new StudentRepresentationModel { name = "Active Student" }
            };

            _mapper.Map<List<StudentRepresentationModel>>(Arg.Any<List<Student>>()).Returns(expectedResponseModels);

            // Act
            var result = await _handler.Handle(new GetAllStudentsRequestModel(), CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.status.ShouldBe(200);
            result.Students.Count.ShouldBe(1);
            result.Students[0].name.ShouldBe("Active Student");
        }
    }
}
