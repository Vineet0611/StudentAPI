using FluentValidation;

namespace StudentApi.Features.StudentFeatures.UpdateStudent
{
    public class UpdateStudentValidator : AbstractValidator<UpdateStudentRequestModel>
    {
        public UpdateStudentValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Student ID is required.");
            RuleFor(x => x.student).NotNull().WithMessage("Student data is required.");
            
            When(x => x.student != null, () => {
                RuleFor(x => x.student.name).NotEmpty().WithMessage("Student name is required.");
                RuleFor(x => x.student.email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
                RuleFor(x => x.student.age).GreaterThan(0).WithMessage("Age must be greater than 0.");
                RuleFor(x => x.student.course).NotEmpty().WithMessage("Course is required.");
                RuleFor(x => x.student.status).IsInEnum().WithMessage("Invalid status.");
            });
        }
    }
}
