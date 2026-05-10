using FluentValidation;

namespace StudentApi.Features.StudentFeatures.AddStudent
{
    public class AddStudentValidator : AbstractValidator<AddStudentRequestModel>
    {
        public AddStudentValidator()
        {
            RuleFor(x => x.student).NotNull().WithMessage("Student data is required.");
            RuleFor(x => x.student.name).NotEmpty().WithMessage("Student name is required.");
            RuleFor(x => x.student.email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
            RuleFor(x => x.student.age).GreaterThan(0).WithMessage("Age must be greater than 0.");
            RuleFor(x => x.student.course).NotEmpty().WithMessage("Course is required.");
        }
    }
}
