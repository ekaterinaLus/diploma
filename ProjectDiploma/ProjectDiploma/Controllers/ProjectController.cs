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
    public class ProjectController : ControllerBase
    {
        private readonly BusinessUniversityContext _context;

        public ProjectController(BusinessUniversityContext context)
        {
            _context = context;
            ProjectStore.context = context;
        }
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
        [HttpPost("[action]")]
        public bool PutProject([FromBody]Project project)
        {
            return ProjectStore.PutProject(project);
        }
    }
   
}