using System.Collections.Generic;
using Diploma.DataBase;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.Logic;
using ProjectDiploma.ViewModel;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase, IPagingController<EventViewModel>
    {
        private readonly BusinessUniversityContext _context;
        private readonly EventModel _model;

        public EventController(BusinessUniversityContext context)
        {
            _context = context;
            _model = new EventModel(context);
        }
        
        [HttpGet("[action]")]
        public int GetCount()
        {
            return _model.GetEventsCount();
        }

        [HttpGet("[action]")]
        public IEnumerable<EventViewModel> GetPage([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            return _model.GetPagingEvents(pageIndex, pageSize);
        }
    }
}