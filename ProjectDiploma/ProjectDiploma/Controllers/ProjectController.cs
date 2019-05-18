using Diploma.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProjectDiploma.Logic;
using ProjectDiploma.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase, IPagingController<ProjectViewModel>
    {
        private readonly BusinessUniversityContext _context;
        private readonly ProjectModel _model;
        private readonly NeuralNetworkModel _nnModel = new NeuralNetworkModel();

        public ProjectController(BusinessUniversityContext context, IMemoryCache memCache)
        {
            _context = context;
            _model = new ProjectModel(context, memCache);
        }

        [HttpGet("[action]")]
        public int GetCount() => _model.GetItemsCount();

        [HttpGet("[action]")]
        public IEnumerable<ProjectViewModel> GetPage([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            _model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return _model.GetPagingItems(pageIndex, pageSize);
        }

        //TODO: interface

        [HttpGet("[action]/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return new JsonResult(_model.Get(id));
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "ADMIN,UNIVERSITY")]
        public IActionResult Add([FromBody] ProjectViewModel item)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(GetErrorsFromModel());
            }
            _model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return new JsonResult(_model.Add(item));
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "ADMIN,UNIVERSITY")]
        public IActionResult Update([FromBody] ProjectViewModel item)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(GetErrorsFromModel());
            }

            _model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return new JsonResult(_model.Update(item));
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "ADMIN,UNIVERSITY")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(GetErrorsFromModel());
            }

            _model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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