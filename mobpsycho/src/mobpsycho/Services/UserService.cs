using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using mobpsycho.Models;
using mobpsycho.Models.Common;
using mobpsycho.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace mobpsycho.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public UserResponse Auth(AuthRequest model)
        {
            UserResponse userResponse = new UserResponse();
            using (var db = new MobpsychoDbContext())
            {
                string spassword = Encrypt.GetSHA256(model.Password);

                var usuario = db.Users.Where(d => d.Email == model.Email
                && d.Password == spassword).FirstOrDefault();

                if (usuario == null) return null;
                
                userResponse.Email = usuario.Email;
                userResponse.Token = GetToken(usuario);
            }
            return userResponse;

        }

        private string GetToken(User usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var llave = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.IdUser.ToString()),
                        new Claim(ClaimTypes.Name, usuario.Email),
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave),SecurityAlgorithms.HmacSha256),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
