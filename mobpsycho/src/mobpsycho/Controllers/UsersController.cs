using AutoMapper;
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
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, MobpsychoDbContext context, IMapper mapper)
        {
            _userService = userService;
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los usuarios registrados con su contraseña en SHA256
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Response>> getUsuarios()
        {
            var usuarios = await _context.Users.ToListAsync();

            var response = _mapper.Map<List<UserResponse>>(usuarios);

            return Ok(new Response(true,"Usuarios registrados en el sistema", response));

        }

        /// <summary>
        /// Get Usuario específico
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        [HttpGet("{idUser}")]
        public async Task<ActionResult<Response>> getUsuario(int idUser)
        {
            var user = await _context.Users.FindAsync(idUser);

            if(user != null)
            {
                var response = _mapper.Map<UserResponse>(user);

                return Ok(response);
            }

            return NotFound();

        }

        /// <summary>
        /// Desencripta la contraseña SHA256 y Obtiene el JWT valido 60 días 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
