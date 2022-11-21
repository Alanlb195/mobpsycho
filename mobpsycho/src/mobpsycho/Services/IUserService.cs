using mobpsycho.Models;
using mobpsycho.Models.Response;

namespace mobpsycho.Services
{
    public interface IUserService
    {
        UserResponse Auth(AuthRequest model);
    }
}
