using DataStore.Entities;
using DataStore.Repositories.OrderedRepository;
using DataStore.Repositories.PagingRepository;

namespace DataStore.Repositories.EventRepository
{
    public interface IEventsRepository: IOrderedRepository<Event>, IPagingRepository<Event>
    {
        
    }
}
