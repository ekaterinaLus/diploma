using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStore.Entities;
using Diploma.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.Logic;

namespace ProjectDiploma.Controllers
{    
    [Route("api/Event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly BusinessUniversityContext _context;

        public EventController(BusinessUniversityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public Event GetRandomEvent()
        {
            return new EventModel(_context).GetRandomEvent();
        }

        [HttpGet]
        public IEnumerable<Event> GetRandom()
        {
            return new EventModel(_context).GetRandomEvents();
        }
    }
}