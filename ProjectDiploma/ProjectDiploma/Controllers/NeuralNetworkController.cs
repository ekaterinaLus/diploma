using Diploma.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.Logic;
using System.Security.Claims;
using System.Linq;
using DataStore.Entities.Projects;
using Microsoft.AspNetCore.Identity;
using DataStore.Entities;
using System.Threading.Tasks;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NeuralNetworkController : ControllerBase
    {
        private readonly BusinessUniversityContext _context;
        private readonly UserManager<User> _userManager;
        private readonly NeuralNetworkModel _model;

        public NeuralNetworkController(UserManager<User> userManager, BusinessUniversityContext context)
        {
            _context = context;
            _userManager = userManager;
            _model = new NeuralNetworkModel(_context); 
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Train([FromQuery] int projectId, [FromQuery] int interest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

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

            _model.Train(projectId, interest, user);
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult PreTrain()
        {
            _model.PreTrain();
            return Ok();
        }

    }
}
