using FluentValidation.Results;

namespace AbcCompany.Core.Domain.Entities
{
    public sealed class ResponseHttp<T> where T : class
    {
        public T Model { get; set; }
        public ValidationResult Validation { get; set; }
        public ResponseHttp()
        {
            Validation = new ValidationResult();
        }


        public void AddError(string message)
        {
            Validation.Errors.Add(new ValidationFailure(string.Empty, message));
        }

        public void AddErrors(ValidationResult validation)
        {
            Validation.Errors.AddRange(validation?.Errors ?? new List<ValidationFailure>());
        }
    }
}
