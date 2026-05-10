
namespace Infastructure.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(IReadOnlyCollection<ValidaionError> _errors):base("Validation failed")
        {
            Errors = _errors;
        }
        public IReadOnlyCollection<ValidaionError> Errors { get; }
    }
    public record ValidaionError(string PropertyName, string ErrorMessage);
}
