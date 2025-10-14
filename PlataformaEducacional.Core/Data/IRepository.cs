using System;
using PlataformaEducacao.Core.Domain;

namespace PlataformaEducacao.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
