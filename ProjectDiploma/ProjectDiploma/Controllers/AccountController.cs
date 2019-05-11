using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataStore.Entities;
using DataStore.Repositories;
using Diploma.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.Entities;
using ProjectDiploma.ViewModel;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly BusinessUniversityContext _dbContext;
                public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, BusinessUniversityContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO: сделать проверку на пользователя
                User user = new User { Email = model.Email, UserName = model.Email };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                var errors = new List<IdentityError>();
                
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(user, model.Role);
                    if (result.Succeeded)
                    {
                        IOrganization organization = null;

                        if (model.Organization.Type == OrganizationViewModel.OrganizationType.Company)
                        {
                            organization = _dbContext.Companies.FirstOrDefault(x => x.Name == model.Organization.Name);
                            if (organization == null)
                            {
                                organization = _dbContext.Companies.Add(new Company { Name = model.Organization.Name, ContactInformation = model.Organization.ContactInformation }).Entity;
                            }                                
                        }
                        else
                        {
                            organization = _dbContext.Universities.FirstOrDefault(x => x.Name == model.Organization.Name);
                            if (organization == null)
                            {
                                organization = _dbContext.Universities.Add(new University { Name = model.Organization.Name, ContactInformation = model.Organization.ContactInformation }).Entity;
                            }
                        }

                        organization.Employees.Add(user);
                        await _dbContext.SaveChangesAsync();

                        var roles = await _userManager.GetRolesAsync(user);
                        var role = roles.FirstOrDefault();

                        return new JsonResult(new Response<UserViewModel>(
                            new UserViewModel
                            {
                                Email = user.Email,
                                Role = role
                            }));
                    }
                    else
                    {
                        errors.AddRange(result.Errors);
                    }
                }
                else
                {
                    errors.AddRange(result.Errors);
                }

                foreach (var error in errors)
                {
                    ModelState.AddModelError("Ошибка при создании пользователя", error.Description);
                }
            }

            var errorResponse = new Response();

            foreach (var error in ModelState)
            {
                errorResponse.AddMessage(MessageType.ERROR, $"{error.Key}: {error.Value.Errors.Aggregate(string.Empty, (x, y) => x += $"{y.ErrorMessage};")}");
            }
            
            return new JsonResult(errorResponse);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    var roles = await _userManager.GetRolesAsync(user);
                    var role = roles.FirstOrDefault();

                    return Ok(new UserViewModel
                    {
                        Email = user.UserName,
                        Role = role
                    });
                }
                else
                {
                    ModelState.AddModelError("LoginError", "Не верный логин или пароль");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}