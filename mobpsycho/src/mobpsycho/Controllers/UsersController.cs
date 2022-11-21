using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mobpsycho.Models;
using mobpsycho.Models.Response;
using mobpsycho.Services;

namespace mobpsycho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public IUserService _userService;
        private readonly MobpsychoDbContext _context;

        public UsersController(IUserService userService, MobpsychoDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> getUsuarios()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("login")]
        public IActionResult Autentificar([FromBody] AuthRequest model)
        {

            var userResponse = _userService.Auth(model);

            if (userResponse == null)
            {
                return BadRequest(new Response(false, "usuario o contraseña incorrectos"));
            }

            return Ok(new Response(true, "Login correcto", userResponse));
        }
    }
}
