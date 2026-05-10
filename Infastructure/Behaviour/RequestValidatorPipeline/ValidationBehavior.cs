using FluentValidation;
using Cortex.Mediator.Commands;
using Azure.Core;

namespace Infastructure.Behaviour.RequestValidatorPipeline
{
    public class ValidationBehavior<TCommand, TResponse> : ICommandPipelineBehavior<TCommand, TResponse> where TCommand : class, ICommand<TResponse> where TResponse : class
    {
        private readonly IEnumerable<IValidator<TCommand>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TCommand>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TCommand request, CommandHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TCommand>(request);
            var validationFailure = await Task.WhenAll(
               _validators.Select(validators => validators.ValidateAsync(context)));
            var errors = validationFailure
                    .Where(validationResult => !validationResult.IsValid)
                    .SelectMany(validationResult => validationResult.Errors)
                    .Select(validationFailure => new Exceptions.ValidaionError(
                    validationFailure.PropertyName,
                    validationFailure.ErrorMessage
                    ))
                    .ToList();
            if (errors.Any())
            {
                throw new Exceptions.ValidationException(errors);
            }
            var response = await next();
            return response;
        }
    }
}
