using System;
using PlataformaEducacional.Core.Domain;

namespace PlataformaEducacional.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
