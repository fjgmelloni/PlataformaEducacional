using MediatR;
using PlataformaEducacional.Core.Messages;
using PlataformaEducacional.Core.Messages.Base;
using System;
using System.Collections.Generic;

namespace PlataformaEducacional.Core.Messages.Base
{
    public abstract class Command : Message, IRequest<bool>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; protected set; } = new();

        protected Command()
        {
            Timestamp = DateTime.UtcNow;
        }

        public virtual bool IsValid() => ValidationResult.IsValid;
    }
    public sealed class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; } = new();

        public void AddError(string message) => Errors.Add(message);
        public void AddErrors(IEnumerable<string> messages) => Errors.AddRange(messages);
    }
}
