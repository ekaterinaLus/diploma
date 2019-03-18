using Diploma.DataBase;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly TagModel _model;

        public TagController(BusinessUniversityContext context)
        {
            _model = new TagModel(context);
        }

        [HttpGet("[action]")]
        public IActionResult Get([FromQuery] int count, [FromQuery] string tagName)
        {
            return new JsonResult(_model.Get(count, tagName));
        }
    }
}
