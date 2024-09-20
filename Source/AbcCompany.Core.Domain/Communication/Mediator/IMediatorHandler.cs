using AbcCompany.Core.Domain.Messages;
using AbcCompany.Core.Domain.Messages.CommonMessages;

namespace AbcCompany.Core.Domain.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T @event) where T : Event;
        Task<bool> SendCommand<T>(T command) where T : Command;
        Task PublishDomainEvent<T>(T notification) where T : DomainEvent;

    }
}
