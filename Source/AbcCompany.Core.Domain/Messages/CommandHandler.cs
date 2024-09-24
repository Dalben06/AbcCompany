using AbcCompany.Core.Domain.Communication.Mediator;
using AbcCompany.Core.Domain.Entities;
using AbcCompany.Core.Domain.Messages.CommonMessages;
using FluentValidation.Results;
using MediatR;

namespace AbcCompany.Core.Domain.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;
        protected IMediatorHandler _mediatorHandler;
        private List<BaseEntity> _eventToPub;

        protected CommandHandler(IMediatorHandler mediatorHandler)
        {
            ValidationResult = new ValidationResult();
            _eventToPub = new List<BaseEntity>();
            _mediatorHandler = mediatorHandler;
        }

        protected void AddError(string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
        }
        protected void AddDomainChanges<T>(T obj) where T : BaseEntity
        {
            if (_eventToPub.Contains(obj)) return;

            _eventToPub.Add(obj);
        }
        protected async Task Commit()
        {
            var domainEvents = _eventToPub.SelectMany(e => e.DomainEvents).ToList();

            var tasks = domainEvents
               .Select(async (domainEvent) => {
                   await _mediatorHandler.PublishEvent(domainEvent);
               });

            await Task.WhenAll(tasks);
        }
    }
}
