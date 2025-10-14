using PlataformaEducacao.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaEducacional.Core.Messages.Common.IntegrationEvents
{
    public abstract class IntegrationEvent : Event
    {
        public Guid CorrelationId { get; }
        public string SourceContext { get; }
        public DateTime OccurredOnUtc { get; }

        protected IntegrationEvent(string sourceContext = "Core")
        {
            CorrelationId = Guid.NewGuid();
            SourceContext = sourceContext;
            OccurredOnUtc = DateTime.UtcNow;
        }
    }
}
