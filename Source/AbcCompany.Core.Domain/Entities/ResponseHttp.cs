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

    }
}
