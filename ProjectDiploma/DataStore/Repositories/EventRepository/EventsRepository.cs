using DataStore.Entities;
using DataStore.Repositories.PagingRepository;
using Diploma.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataStore.Repositories.EventRepository
{
    public class EventsRepository: DatePagingRepository<Event>, IEventsRepository
    {
        public EventsRepository(BusinessUniversityContext dbContext) : base(dbContext) { }
        
        public override IQueryable<Event> GetAll()
        {
            return DbContext.Events.Include(x => x.Tags)
                .ThenInclude(x => x.Tags);
        }
    }
}



