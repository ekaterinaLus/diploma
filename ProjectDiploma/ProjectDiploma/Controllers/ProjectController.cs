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
    [Route("api/Project")]
    public class ProjectController : ControllerBase
    {
        [HttpGet("[action]")]
        public IEnumerable<Project>  GetAllProject()
        {
            return ProjectStore.GetProject();
        }

        [HttpGet("[action]")]
        public Project GetRandomProject()
        {
            return ProjectStore.GetRndmProject();
        }
        
    }
}