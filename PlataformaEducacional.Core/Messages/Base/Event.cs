using MediatR;
using PlataformaEducacao.Core.Messages.Base;

namespace PlataformaEducacao.Core.Messages
{
    public abstract class Command : Message, IRequest<bool> { protected Command() : base() { } }
    public abstract class Event : Message, INotification { protected Event() : base() { } }
}
