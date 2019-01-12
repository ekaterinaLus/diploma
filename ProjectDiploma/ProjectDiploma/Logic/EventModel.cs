using DataStore.Entities;
using DataStore.Repositories.EventRepository;
using Diploma.DataBase;
using ProjectDiploma.ViewModel;
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
        
        public int GetEventsCount()
        {
            return _eventRepository.GetItemsCount();
        }

        public IEnumerable<EventViewModel> GetPagingEvents(int pageIndex, int pageSize)
        {
            var items = _eventRepository.GetPaging(pageIndex, pageSize).ToArray();
            return items.Select(item => EventViewModel.FromDbObject(item));
        }
    }
}
