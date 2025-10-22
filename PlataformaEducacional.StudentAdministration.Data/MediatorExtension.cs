using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.StudentAdministration.Data
{
    public static class MediatorExtension
    {
        public static async Task PublishDomainEvents(this IMediatorHandler mediator, StudentAdministrationContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents.Select(async (domainEvent) =>
            {
                await mediator.PublishEvent(domainEvent);
            });

            await Task.WhenAll(tasks);
        }
    }
}
