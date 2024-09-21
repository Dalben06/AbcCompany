using AbcCompany.Core.Domain.Messages;
using AbcCompany.Core.Domain.Messages.CommonMessages;
using FluentValidation.Results;

namespace AbcCompany.Core.Domain.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T @event) where T : Event;
        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
        Task PublishDomainEvent<T>(T notification) where T : DomainEvent;

    }
}
