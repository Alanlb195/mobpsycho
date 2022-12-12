using AutoMapper;
using mobpsycho.Models;

namespace mobpsycho.Tools
{
    public class MappingProfile: Profile
    {
        // Parametro 1: La clase que se recibe
        // Parámetro 2: La clase que se mapea/setea con los atributos de la primera
        public MappingProfile()
        {
            CreateMap<CharacterRequest, Character>();
            CreateMap<Character, CharacterRequest>();

            CreateMap<AbilitieRequest, Abilitie>(); // Para put y post
            CreateMap<Abilitie, AbilitieRequest>(); // Para Get y Get All

            CreateMap<User, UserResponse>();
            CreateMap<UserResponse, User>();
        }
    }
}
