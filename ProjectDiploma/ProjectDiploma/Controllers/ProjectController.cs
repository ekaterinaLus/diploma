using System.Collections.Generic;
using System.Linq;
using Diploma.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.Logic;
using ProjectDiploma.ViewModel;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase, IPagingController<ProjectViewModel>
    {
        private readonly BusinessUniversityContext _context;
        private readonly ProjectModel _model;

        public ProjectController(BusinessUniversityContext context)
        {
            _context = context;
            _model = new ProjectModel(context);
        }

        [HttpGet("[action]")]
        public int GetCount() => _model.GetItemsCount();

        [HttpGet("[action]")]
        public IEnumerable<ProjectViewModel> GetPage([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            return _model.GetPagingItems(pageIndex, pageSize);
        }

        //TODO: interface

        [HttpGet("[action]/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return new JsonResult(_model.Get(id));
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] ProjectViewModel item)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(GetErrorsFromModel());
            }

            return new JsonResult(_model.Add(item));
        }

        [HttpPut("[action]")]
        public IActionResult Update([FromBody] ProjectViewModel item)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(GetErrorsFromModel());
            }

            return new JsonResult(_model.Update(item));
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(GetErrorsFromModel());
            }

            return new JsonResult(_model.Delete(id));
        }

        private Response GetErrorsFromModel()
        {
            var errors = ModelState.Values.SelectMany(m => m.Errors);
            var result = new Response();

            foreach (var error in errors)
            {
                result.AddMessage(MessageType.ERROR, error.ErrorMessage);
            }

            return result;
        }
    }
   
}