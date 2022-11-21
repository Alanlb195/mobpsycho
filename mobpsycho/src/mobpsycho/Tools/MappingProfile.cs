using AutoMapper;
using mobpsycho.Models;

namespace mobpsycho.Tools
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CharacterRequest, Character>();
        }
    }
}
