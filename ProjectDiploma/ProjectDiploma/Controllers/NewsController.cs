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
    //[Route("api/[controller]")]
    [Produces("application/json")]
    [Route("api/News")]
    public class NewsController : ControllerBase
    {
        [HttpGet("[action]")]
        public News GetRandomNews()
        {
            return NewsStore.GetRndmNews();
        }
    }
}