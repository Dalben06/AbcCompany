namespace AbcCompany.Core.Domain.Messages.CommonMessages
{
    public abstract class DomainEvent : Event
    {
        public DomainEvent(Guid MessageId) : base()
        {
            this.MessageId = MessageId;
        }
    }
}
