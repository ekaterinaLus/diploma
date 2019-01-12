using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.ViewModel;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                        // установка куки
                        //await _signInManager.SignInAsync(user, false);

                        var roles = await _userManager.GetRolesAsync(user);
                        var role = roles.FirstOrDefault();

                        return Ok(new UserViewModel
                        {
                            Email = user.Email,
                            Role = role
                        });
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
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return BadRequest(ModelState);
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