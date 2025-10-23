using MediatR;
using PlataformaEducacional.Core.Messages.Base;


namespace PlataformaEducacional.Core.Messages.Base
{
    public abstract class Event :Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
