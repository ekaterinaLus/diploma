using Diploma.DataBase;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.Logic;
using System.Security.Claims;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("[action]")]
        public void Train([FromQuery] int projectId, [FromQuery] int interest)
        {
            _model.Train(projectId, interest, User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

    }
}
