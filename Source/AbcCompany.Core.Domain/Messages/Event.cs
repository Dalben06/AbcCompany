using MediatR;

namespace AbcCompany.Core.Domain.Messages
{
    public abstract class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            MessageId = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }
    }
}
