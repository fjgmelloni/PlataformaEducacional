using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Domain;

namespace PlataformaEducacional.StudentAdministration.Data
{
    public static class MediatorExtension
    {
        public static async Task PublishDomainEvents(this IMediatorHandler mediator, StudentAdministrationContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents.Select(async domainEvent =>
            {
                await mediator.PublishEventAsync(domainEvent);
            });

            await Task.WhenAll(tasks);
        }
    }
}
