using FluentValidation;

namespace StudentApi.Features.StudentFeatures.DeleteStudent
{
    public class DeleteStudentValidator : AbstractValidator<DeleteStudentRequestModel>
    {
        public DeleteStudentValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Student ID is required.");
        }
    }
}
