using ApiProject.DTO;
using ApiProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(UserDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var user = new AppUser
                {
                    FullName = model.FullName,
                    UserName = model.UserName,
                    Email = model.Email,
                    DateAdded = DateTime.Now,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if(result.Succeeded)
                {
                    return StatusCode(201);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest(new { message = "Email hatalı" });
            }
            else
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if(result.Succeeded)
                {
                    return Ok(new { token = "token" }); // giriş yapan kullanıcı için jwt token üretildi
                }
                else
                {
                    return Unauthorized(); // yetkisizlik durumu tespit edildi ve http 401 kaynak kodu döndürüldü
                }
            }
        }
    }
}

