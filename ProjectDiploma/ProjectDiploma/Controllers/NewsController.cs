using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStore.Entities;
using Diploma.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.Logic;
using ProjectDiploma.ViewModel;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase, IPagingController<NewsViewModel>
    {
        private readonly BusinessUniversityContext _context;
        private readonly NewsModel _model;

        public NewsController(BusinessUniversityContext context)
        {
            _context = context;
            _model = new NewsModel(context);
        }

        [HttpGet("[action]")]
        public int GetCount()
        {
            return _model.GetItemsCount();
        }

        [HttpGet("[action]")]
        public IEnumerable<NewsViewModel> GetPage([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            return _model.GetPagingItems(pageIndex, pageSize);
        }

        [HttpGet("[action]/{id}")]
        public NewsViewModel Get([FromRoute] int id)
        {
            return _model.Get(id);
        }
    }
}