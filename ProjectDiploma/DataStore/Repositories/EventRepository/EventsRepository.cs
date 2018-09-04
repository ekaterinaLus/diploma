using DataStore.Entities;
using Diploma.DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStore.Repositories.EventRepository
{
    class EventsRepository: GenericRepository<Event>, IEventsRepository 
    {
        public EventsRepository(BusinessUniversityContext dbContext) : base(dbContext) { }
        
        public override IEnumerable<Event> GetAll()
        {
            return DbContext.
                Events.Include(x => x.Title);
        }
    }
}



