using DataStore.Entities;
using DataStore.Repositories;
using Diploma.DataBase;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.ViewModel;
using SharedLogic.Mapper;
using System.Collections.Generic;
using System.Linq;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private BusinessUniversityContext _dbContext;

        public OrganizationController(BusinessUniversityContext context)
        {
            _dbContext = context;
        }
        
        [HttpGet("[action]")]
        public IEnumerable<OrganizationViewModel> GetAll()
        {
            var universityRepo = new GenericRepository<University>(_dbContext);
            var businessRepo = new GenericRepository<Company>(_dbContext);

            var result1 = universityRepo.GetAll().ToArray<IOrganization>();
            var result2 = businessRepo.GetAll().ToArray<IOrganization>();

            var result = result1.Union(result2);

            return result.Select(x => x.ToType<OrganizationViewModel>());
        }
    }
}
