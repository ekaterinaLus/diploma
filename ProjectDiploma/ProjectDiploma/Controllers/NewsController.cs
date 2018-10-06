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
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly BusinessUniversityContext _context;

        public NewsController(BusinessUniversityContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public News GetRandomNews()
        {
            return new NewsModel(_context).GetRandomNews();
        }
    }
}