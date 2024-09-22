using AbcCompany.Core.Domain.Entities;
using FluentValidation.Results;
using MediatR;

namespace AbcCompany.Core.Domain.Messages
{
    public abstract class Command<T> : Message, IRequest<ResponseHttp<T>> where T : class
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException("Must need to implement IsValid Method in your Command");
        }
    }
}
