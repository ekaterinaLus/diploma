using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.Logic;

namespace ProjectDiploma.Controllers
{
    [Produces("application/json")]
    [Route("api/Event")]
    public class EventController : ControllerBase
    {
        [HttpGet("[action]")]
        public Event GetRandomEvent()
        {
            return EventStore.GetRndmEvent();
        }
        [HttpGet("[action]")]
        public IEnumerable<Event> GetRandom()
        {
            return EventStore.GtRndmEvnt();
        }
    }
}