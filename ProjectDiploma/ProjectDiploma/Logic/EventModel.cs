using DataStore.Entities;
using DataStore.Repositories.EventRepository;
using Diploma.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectDiploma.Logic
{
    public class EventModel
    {               
        private readonly EventsRepository _eventRepository;

        public EventModel(BusinessUniversityContext dbContext)
        {
            _eventRepository = new EventsRepository(dbContext);            
        }

        public Event GetRandomEvent()
        {
            return _eventRepository.GetRandomEntities().FirstOrDefault();
        }

        public IEnumerable<Event> GetRandomEvents()
        {
            return _eventRepository.GetRandomEntities(2);            
        }
    }
}
