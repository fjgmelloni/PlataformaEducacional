using MediatR;
using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacional.Core.Messages
{
    public abstract class Command : Message, IRequest<bool> { protected Command() : base() { } }
    public abstract class Event : Message, INotification { protected Event() : base() { } }
}
