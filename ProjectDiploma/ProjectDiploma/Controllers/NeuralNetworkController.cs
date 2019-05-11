using Diploma.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.Logic;
using System.Security.Claims;
using System.Linq;
using DataStore.Entities.Projects;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NeuralNetworkController : ControllerBase
    {
        private readonly BusinessUniversityContext _context;
        private readonly NeuralNetworkModel _model;

        public NeuralNetworkController(BusinessUniversityContext context)
        {
            _context = context;
            _model = new NeuralNetworkModel(_context); 
        }

        [HttpPost("[action]")]
        public IActionResult Train([FromQuery] int projectId, [FromQuery] int interest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ProjectRate rate = null; 
            if ((rate = _context.ProjectsRates.FirstOrDefault(x => x.ProjectId == projectId && x.UserId == userId)) == null)
            {
                _context.ProjectsRates.Add(new ProjectRate { ProjectId = projectId, UserId = userId, Rate = interest });
                _context.SaveChanges();
            }
            else if (rate.Rate != interest)
            {
                rate.Rate = interest;
                _context.SaveChanges();
            }

            _model.Train(projectId, interest, userId);
            return Ok();
        }

    }
}
