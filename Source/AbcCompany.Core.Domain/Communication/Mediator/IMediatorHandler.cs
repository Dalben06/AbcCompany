using AbcCompany.Core.Domain.Entities;
using AbcCompany.Core.Domain.Messages;
using AbcCompany.Core.Domain.Messages.CommonMessages;

namespace AbcCompany.Core.Domain.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T @event) where T : Event;
        Task<ResponseHttp<T2>> SendCommand<T, T2>(T command) where T2 : class where T : Command<T2>;
        Task PublishDomainEvent<T>(T notification) where T : DomainEvent;

    }
}
