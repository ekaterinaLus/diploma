using DataStore.Entities;
using DataStore.Repositories.EventRepository;
using Diploma.DataBase;
using ProjectDiploma.ViewModel;

namespace ProjectDiploma.Logic
{
    public class EventModel: PagingModel<EventsRepository, Event, EventViewModel>
    {               
        private readonly EventsRepository _eventRepository;

        protected override EventsRepository Repository => _eventRepository;

        public EventModel(BusinessUniversityContext dbContext)
        {
            _eventRepository = new EventsRepository(dbContext);            
        }
    }
}

