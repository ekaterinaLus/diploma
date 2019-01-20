using System.Collections.Generic;
using Diploma.DataBase;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.Logic;
using ProjectDiploma.ViewModel;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IEnumerable<ProjectViewModel> GetPage(int pageIndex, int pageSize)
        {
            return _model.GetPagingItems(pageIndex, pageSize);
        }
    }
   
}