﻿using DataStore.Entities;
using DataStore.Repositories;
using Diploma.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProjectDiploma.Logic;
using ProjectDiploma.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly BusinessUniversityContext _context;
        private readonly ProjectModel _model;
        private readonly UserManager<User> _userManager;
        private readonly UserRepository _userRepository;
        private readonly NeuralNetworkModel _nnModel = new NeuralNetworkModel();

        public ProjectController(BusinessUniversityContext context, UserManager<User> userManager, IMemoryCache memCache)
        {
            _context = context;
            _userManager = userManager;
            _model = new ProjectModel(context, memCache);
            _userRepository = new UserRepository(context);
        }

        [HttpGet("[action]")]
        public int GetCount() => _model.GetItemsCount();

        [HttpGet("[action]")]
        public async Task<IEnumerable<ProjectViewModel>> GetPage([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            await SetupUserInfo();

            return _model.GetPagingItems(pageIndex, pageSize);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "ADMIN,UNIVERSITY")]
        public async Task<IEnumerable<ProjectViewModel>> GetProjectsByUser()
        {
            await SetupUserInfo();

            return _model.GetProjectByUser();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "ADMIN,UNIVERSITY")]
        public async Task<int> GetSubscribersCount()
        {
            await SetupUserInfo();

            return _model.GetSubscribesHistory();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "ADMIN,UNIVERSITY")]
        public async Task CleanHistory()
        {
            await SetupUserInfo();

            _model.CleanHistory();
        }

        //TODO: interface GetProjectViews

        [HttpGet("[action]/{id}")]
        public async Task AddViewsToProject([FromRoute] int id)
        {
            await SetupUserInfo();

            _model.AddViewsToProject(id);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProjectViews()
        {
            await SetupUserInfo();

            return new JsonResult(_model.GetProjectViews());
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            await SetupUserInfo();

            return new JsonResult(_model.Get(id));
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "ADMIN,UNIVERSITY")]
        public async Task<IActionResult> Add([FromBody] ProjectViewModel item)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(GetErrorsFromModel());
            }

            await SetupUserInfo();

            return new JsonResult(_model.Add(item));
        }

        [HttpPost("[action]/{id}")]
        [Authorize(Roles = "ADMIN,BUSINESS")]
        public async Task<IActionResult> Subscribe([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(GetErrorsFromModel());
            }

            await SetupUserInfo();

            return new JsonResult(_model.Subscribe(id));
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "ADMIN,UNIVERSITY")]
        public async Task<IActionResult> Update([FromBody] ProjectViewModel item)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(GetErrorsFromModel());
            }

            await SetupUserInfo();

            return new JsonResult(_model.Update(item));
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "ADMIN,UNIVERSITY")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(GetErrorsFromModel());
            }

            await SetupUserInfo();

            return new JsonResult(_model.Delete(id));
        }

        private async Task SetupUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userRepository.Get(userId);
            _model.User = user;

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();
                _model.IsBusiness = role.ToUpper() == nameof(BusinessUniversityContext.RoleValues.BUSINESS);
            }
        }

        private Response GetErrorsFromModel()
        {
            var errors = ModelState.Values.SelectMany(m => m.Errors);
            var result = new Response();

            foreach (var error in errors)
            {
                result.AddMessage(MessageType.ERROR, error.ErrorMessage);
            }

            return result;
        }

        //[HttpPost("[action]")]
        //public async Task<IActionResult> Open([FromQuery] int projectId)
        //{
        //    if (_context.Projects.Select(x => x.Id == projectId))
        //    { }
        //    return new JsonResult(_model.Get(projectId));
        //}
    }
   
}