using System.Linq;
using System.Threading.Tasks;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Domain;

namespace PlataformaEducacional.FinancialManagement.Data
{
    public static class MediatorExtension
    {
        public static async Task PublishEvents(this IMediatorHandler mediator, PaymentContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities
                .ToList()
                .ForEach(entry => entry.Entity.ClearDomainEvents());

            var tasks = domainEvents.Select(async domainEvent =>
            {
                await mediator.PublishEventAsync(domainEvent);
            });

            await Task.WhenAll(tasks);
        }
    }
}
