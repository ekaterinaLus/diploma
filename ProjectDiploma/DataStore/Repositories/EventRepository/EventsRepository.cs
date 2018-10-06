using DataStore.Entities;
using DataStore.Repositories.RandomizeRepository;
using Diploma.DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore.Repositories.EventRepository
{
    public class EventsRepository: RandomizeRepository<Event>, IEventsRepository 
    {
        public EventsRepository(BusinessUniversityContext dbContext) : base(dbContext) { }
        
        public override IEnumerable<Event> GetAll()
        {
            return DbContext.
                Events.Include(x => x.Tags);
        }
    }
}



