using AbcCompany.Core.Domain.Entities;
using AbcCompany.Core.Domain.Messages;
using AbcCompany.Core.Domain.Messages.CommonMessages;
using FluentValidation.Results;
using MediatR;

namespace AbcCompany.Core.Domain.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishEvent<T>(T @event) where T : Event
        {
            await _mediator.Publish(@event);
        }

        public async Task<ResponseHttp<T2>> SendCommand<T,T2>(T command) where T2 : class  where T : Command<T2>
        {
            return await _mediator.Send(command);
        }

        public async Task PublishDomainEvent<T>(T notification) where T : DomainEvent
        {
            await _mediator.Publish(notification);
        }
    }
}
