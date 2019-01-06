using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStore.Entities;
using Diploma.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.Logic;
using ProjectDiploma.ViewModel;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly BusinessUniversityContext _context;

        public EventController(BusinessUniversityContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        [Authorize]
        public EventViewModel GetRandomEvent()
        {
            var result = new EventModel(_context).GetRandomEvent();
            return EventViewModel.FromDbObject(result);
        }

        [HttpGet("[action]")]
        public IEnumerable<EventViewModel> GetRandom()
        {
            return new EventModel(_context).GetRandomEvents().Select(x => EventViewModel.FromDbObject(x));
        }
    }
}