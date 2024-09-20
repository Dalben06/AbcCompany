namespace AbcCompany.Core.Domain.Messages
{
    public abstract class Message
    {
        public string MessageType { get; protected set; }
        public Guid MessageId { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
